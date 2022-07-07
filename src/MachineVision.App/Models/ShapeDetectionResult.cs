using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineVision.App.Models
{
    public class ShapeDetectionResult
    {
        public List<(string shape,int count)> Description { get; set; }
        public Mat ImageResult { get; set; }
        public ShapeDetectionResult()
        {
            Description = new List<(string, int)>();
            
        }
    }
}
