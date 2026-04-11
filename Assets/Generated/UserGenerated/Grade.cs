using System.Linq;
using UnityEngine;

namespace Tables
{
    public partial class Grade : IGradeType, IIconAtlasSprite
    {
        public static Grade GetGradeByType(GradeType gradeType) => Table.Values.First(grade => grade.gradeType == gradeType);
    }
}
