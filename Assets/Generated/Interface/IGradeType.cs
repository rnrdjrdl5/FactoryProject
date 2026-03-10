using UnityEngine;

public interface IGradeType
{
    Tables.GradeType gradeType { get; set; }

    public Tables.Grade Grade => Tables.Grade.GetGradeByType(gradeType);
}
