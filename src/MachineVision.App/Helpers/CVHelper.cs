using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Size = System.Drawing.Size;
using Color = System.Drawing.Color;
using PointCollection = Emgu.CV.PointCollection;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;
using MachineVision.App.Models;

namespace MachineVision.App.Helpers
{
    public class CVHelper
    {
        public static ShapeDetectionResult DetectCircleContour(Mat img)
        {
            /*
            img_color = cv2.imread('yNxlz.jpg')
            img_gray = cv2.cvtColor(img_color, cv2.COLOR_BGR2GRAY) ok

            image = cv2.GaussianBlur(img_gray, (5, 5), 0) ok

            thresh = cv2.adaptiveThreshold(image,255,cv2.ADAPTIVE_THRESH_MEAN_C,\
            cv2.THRESH_BINARY_INV,11,2) ok

            contours,hierarchy = cv2.findContours(thresh.copy(), cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
            cnt = contours

            contour_list = []
            for contour in contours:
                approx = cv2.approxPolyDP(contour,0.01*cv2.arcLength(contour,True),True)
                area = cv2.contourArea(contour)
                # Filter based on length and area
                if (7 < len(approx) < 18) & (900 >area > 200):
                    # print area
                    contour_list.append(contour)

            cv2.drawContours(img_color, contour_list,  -1, (255,0,0), 2)
            cv2.imshow('Objects Detected',img_color)
            cv2.waitKey(5000)


             */
            var res = new ShapeDetectionResult();
            using (UMat gray = new UMat())
            using (UMat cannyEdges = new UMat())
            using (Mat circleImage = new Mat(img.Size, DepthType.Cv8U, 3)) //image to draw circles on
            {
                //Convert the image to grayscale and filter out the noise
                CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray);

                //Remove noise
                CvInvoke.GaussianBlur(gray, gray, new Size(5, 5),0);
                
                //adaptive th
                CvInvoke.AdaptiveThreshold(gray, gray, 255, AdaptiveThresholdType.MeanC, ThresholdType.BinaryInv, 11, 2);

                //find contour
                UMat hirarki = new UMat();
                Emgu.CV.Util.VectorOfVectorOfPoint vecOfVecOfPoint = new Emgu.CV.Util.VectorOfVectorOfPoint();
                CvInvoke.FindContours(gray.Clone(), vecOfVecOfPoint, hirarki, RetrType.External, ChainApproxMethod.ChainApproxSimple);
                var contours = vecOfVecOfPoint.ToArrayOfArray();
                var contour_list = new List<Point[]>();
                foreach (var contour in contours)
                {
                    Emgu.CV.Util.VectorOfPoint point = new VectorOfPoint(contour);
                    Emgu.CV.Util.VectorOfPoint approx = new VectorOfPoint ();
                    CvInvoke.ApproxPolyDP(point, approx, 0.01 * CvInvoke.ArcLength(point, true), true);
                    var area = CvInvoke.ContourArea(point);
                    //# Filter based on length and area
                    if (((7 < approx.Length) && (approx.Length < 18)) & ((900 > area) && (area > 200))){
                        contour_list.Add(contour);
                    }
                }
                img.CopyTo(circleImage);
                VectorOfVectorOfPoint contour_list_vector = new VectorOfVectorOfPoint(contour_list.ToArray());
                CvInvoke.DrawContours(circleImage, contour_list_vector, -1, new MCvScalar(255, 0, 0), 2);

                //Drawing a light gray frame around the image
                CvInvoke.Rectangle(circleImage,
                    new Rectangle(Point.Empty, new Size(circleImage.Width - 1, circleImage.Height - 1)),
                    new MCvScalar(120, 120, 120));
                //Draw the labels
                CvInvoke.PutText(circleImage, "Circles", new Point(20, 20), FontFace.HersheyDuplex, 0.5,
                    new MCvScalar(120, 120, 120));
                
                res.Description.Add(("circle", contour_list.Count));
                Mat result = new Mat();
                CvInvoke.VConcat(circleImage, result);
                res.ImageResult = result;
                return res;
            }
        }
        
        public static ShapeDetectionResult DetectCircleHough(Mat img)
        {
            /*
            import cv2 as cv2

            img_color = cv2.imread('yNxlz.jpg')
            img_gray = cv2.cvtColor(img_color, cv2.COLOR_BGR2GRAY) ok

            img_gray = cv2.GaussianBlur(img_gray, (7, 7), 0) ok

            #Hough circle
            circles = cv2.HoughCircles(img_gray, cv2.cv.CV_HOUGH_GRADIENT, 1, minDist=15,
                    param1=50, param2=18, minRadius=12, maxRadius=22)

            if circles is not None:
            for i in circles[0, :]:
            # draw the outer circle
            cv2.circle(img_color, (i[0], i[1]), i[2], (0, 255, 0), 2)
            # draw the center of the circle
            cv2.circle(img_color, (i[0], i[1]), 2, (0, 0, 255), 3)

            cv2.imwrite('with_circles.png', img_color)

            cv2.imshow('circles', img_color)
            cv2.waitKey(5000)

             */
            var res = new ShapeDetectionResult();
            using (UMat gray = new UMat())
            using (UMat cannyEdges = new UMat())
            using (Mat circleImage = new Mat(img.Size, DepthType.Cv8U, 3)) //image to draw circles on
            {
                //Convert the image to grayscale and filter out the noise
                CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray);

                //Remove noise
                CvInvoke.GaussianBlur(gray, gray, new Size(7, 7),0);


                #region circle detection
                
                CircleF[] circles = CvInvoke.HoughCircles(gray, HoughModes.Gradient, 1.0, minDist : 15,
                    param1 : 50, param2 : 18, minRadius : 12, maxRadius : 22);
                #endregion

                #region draw circles
                img.CopyTo(circleImage);

                //circleImage.SetTo(new MCvScalar(0));
                foreach (CircleF circle in circles)
                {
                    //outer
                    CvInvoke.Circle(circleImage, Point.Round(circle.Center), (int)circle.Radius,
                        new Bgr(Color.Blue).MCvScalar, 2);
                    //center
                    CvInvoke.Circle(circleImage, Point.Round(circle.Center), 2,
                        new Bgr(Color.Red).MCvScalar, 3);
                }
                //Drawing a light gray frame around the image
                CvInvoke.Rectangle(circleImage,
                    new Rectangle(Point.Empty, new Size(circleImage.Width - 1, circleImage.Height - 1)),
                    new MCvScalar(120, 120, 120));
                //Draw the labels
                CvInvoke.PutText(circleImage, "Circles", new Point(20, 20), FontFace.HersheyDuplex, 0.5,
                    new MCvScalar(120, 120, 120));
                #endregion
                res.Description.Add(("circle", circles.Length));
                Mat result = new Mat();
                CvInvoke.VConcat(circleImage, result);
                res.ImageResult = result;
                return res;
            }
        }
        public static ShapeDetectionResult DetectShape(Mat img,bool DetectCircle=true, bool DetectTriangleAndRectangle=true,bool DetectEdge=true)
        {
            var res = new ShapeDetectionResult();
            using (UMat gray = new UMat())
            using (UMat cannyEdges = new UMat())
            using (Mat triangleRectangleImage = new Mat(img.Size, DepthType.Cv8U, 3)) //image to draw triangles and rectangles on
            using (Mat circleImage = new Mat(img.Size, DepthType.Cv8U, 3)) //image to draw circles on
            using (Mat lineImage = new Mat(img.Size, DepthType.Cv8U, 3)) //image to draw lines on
            {
                //Convert the image to grayscale and filter out the noise
                CvInvoke.CvtColor(img, gray, ColorConversion.Bgr2Gray);

                //Remove noise
                CvInvoke.GaussianBlur(gray, gray, new Size(3, 3), 1);
                
                double cannyThreshold = 180.0;
                if (DetectCircle)
                {
                    #region circle detection
                    //double cannyThreshold = 180.0;
                    double circleAccumulatorThreshold = 120;
                    CircleF[] circles = CvInvoke.HoughCircles(gray, HoughModes.Gradient, 2.0, 20.0, cannyThreshold,
                        circleAccumulatorThreshold, 5);
                    #endregion

                    #region draw circles
                    circleImage.SetTo(new MCvScalar(0));
                    foreach (CircleF circle in circles)
                        CvInvoke.Circle(circleImage, Point.Round(circle.Center), (int)circle.Radius,
                            new Bgr(Color.Red).MCvScalar, 2);

                    //Drawing a light gray frame around the image
                    CvInvoke.Rectangle(circleImage,
                        new Rectangle(Point.Empty, new Size(circleImage.Width - 1, circleImage.Height - 1)),
                        new MCvScalar(120, 120, 120));
                    //Draw the labels
                    CvInvoke.PutText(circleImage, "Circles", new Point(20, 20), FontFace.HersheyDuplex, 0.5,
                        new MCvScalar(120, 120, 120));
                    #endregion
                    res.Description.Add(("circle", circles.Length));
                }
                if (DetectEdge)
                {
                    #region Canny and edge detection
                    double cannyThresholdLinking = 120.0;
                    CvInvoke.Canny(gray, cannyEdges, cannyThreshold, cannyThresholdLinking);
                    LineSegment2D[] lines = CvInvoke.HoughLinesP(
                        cannyEdges,
                        1, //Distance resolution in pixel-related units
                        Math.PI / 45.0, //Angle resolution measured in radians.
                        20, //threshold
                        30, //min Line width
                        10); //gap between lines
                    #endregion

                    
                    #region draw lines
                    lineImage.SetTo(new MCvScalar(0));
                    foreach (LineSegment2D line in lines)
                        CvInvoke.Line(lineImage, line.P1, line.P2, new Bgr(Color.LightGreen).MCvScalar, 2);
                    //Drawing a light gray frame around the image
                    CvInvoke.Rectangle(lineImage,
                        new Rectangle(Point.Empty, new Size(lineImage.Width - 1, lineImage.Height - 1)),
                        new MCvScalar(120, 120, 120));
                    //Draw the labels
                    CvInvoke.PutText(lineImage, "Lines", new Point(20, 20), FontFace.HersheyDuplex, 0.5,
                        new MCvScalar(120, 120, 120));
                    #endregion
                    res.Description.Add(("edge", lines.Length));
                }

                if (DetectTriangleAndRectangle)
                {
                    #region Find triangles and rectangles
                    List<Triangle2DF> triangleList = new List<Triangle2DF>();
                    List<RotatedRect> boxList = new List<RotatedRect>(); //a box is a rotated rectangle
                    using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
                    {
                        CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List,
                            ChainApproxMethod.ChainApproxSimple);
                        int count = contours.Size;
                        for (int i = 0; i < count; i++)
                        {
                            using (VectorOfPoint contour = contours[i])
                            using (VectorOfPoint approxContour = new VectorOfPoint())
                            {
                                CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05,
                                    true);
                                if (CvInvoke.ContourArea(approxContour, false) > 250
                                ) //only consider contours with area greater than 250
                                {
                                    if (approxContour.Size == 3) //The contour has 3 vertices, it is a triangle
                                    {
                                        Point[] pts = approxContour.ToArray();
                                        triangleList.Add(new Triangle2DF(
                                            pts[0],
                                            pts[1],
                                            pts[2]
                                        ));
                                    }
                                    else if (approxContour.Size == 4) //The contour has 4 vertices.
                                    {
                                        #region determine if all the angles in the contour are within [80, 100] degree
                                        bool isRectangle = true;
                                        Point[] pts = approxContour.ToArray();
                                        LineSegment2D[] edges = PointCollection.PolyLine(pts, true);

                                        for (int j = 0; j < edges.Length; j++)
                                        {
                                            double angle = Math.Abs(
                                                edges[(j + 1) % edges.Length].GetExteriorAngleDegree(edges[j]));
                                            if (angle < 80 || angle > 100)
                                            {
                                                isRectangle = false;
                                                break;
                                            }
                                        }

                                        #endregion

                                        if (isRectangle) boxList.Add(CvInvoke.MinAreaRect(approxContour));
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    #region draw triangles and rectangles
                    triangleRectangleImage.SetTo(new MCvScalar(0));
                    foreach (Triangle2DF triangle in triangleList)
                    {
                        CvInvoke.Polylines(triangleRectangleImage, Array.ConvertAll(triangle.GetVertices(), Point.Round),
                            true, new Bgr(Color.LightBlue).MCvScalar, 2);
                    }

                    foreach (RotatedRect box in boxList)
                    {
                        CvInvoke.Polylines(triangleRectangleImage, Array.ConvertAll(box.GetVertices(), Point.Round), true,
                            new Bgr(Color.Orange).MCvScalar, 2);
                    }

                    //Drawing a light gray frame around the image
                    CvInvoke.Rectangle(triangleRectangleImage,
                        new Rectangle(Point.Empty,
                            new Size(triangleRectangleImage.Width - 1, triangleRectangleImage.Height - 1)),
                        new MCvScalar(120, 120, 120));
                    //Draw the labels
                    CvInvoke.PutText(triangleRectangleImage, "Triangles and Rectangles", new Point(20, 20),
                        FontFace.HersheyDuplex, 0.5, new MCvScalar(120, 120, 120));
                    #endregion
                    res.Description.Add(("triangle", triangleList.Count));
                    res.Description.Add(("rectangle", boxList.Count));

                }
                List<Mat> images = new List<Mat>();
                images.Add(img);
                if (DetectCircle) images.Add(circleImage);
                if (DetectTriangleAndRectangle) images.Add(triangleRectangleImage);
                if (DetectEdge) images.Add(lineImage);
                Mat result = new Mat();
                //CvInvoke.VConcat(new Mat[] { img, triangleRectangleImage, circleImage, lineImage }, result);
                CvInvoke.VConcat(images.ToArray(), result);

                res.ImageResult = result;
                return res;
            }
        }

    }
}
