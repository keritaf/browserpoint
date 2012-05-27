namespace Itransition.BrowserPoint.Model.Entities
{
    using System.ComponentModel.DataAnnotations;

    [ComplexType]
    public class BlockInfo
    {
        public int? Top { get; set; }
        public int? Left { get; set; }

        public int? Width { get; set; }
        public int? Height { get; set; }

        public int? Rotation { get; set; }
        public int? ZIndex { get; set; }
    }
}
