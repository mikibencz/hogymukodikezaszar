using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class patkany : MonoBehaviour {

    int workDayStart = 9;
    int workDayEnd = 17;
    int availableWorkingMinutesOnCurrentWorkingDay = 0;

    DateTime TaskAssigner(DateTime DateTaskReceived, int HoursRequiredForTask)
    {
        int minutesRequiredToFinishTask = HoursRequiredForTask * 60;
        DateTime sortingDate = DateTaskReceived;

        while (0 < minutesRequiredToFinishTask)
        {

            if (sortingDate.DayOfWeek == DayOfWeek.Saturday || sortingDate.DayOfWeek == DayOfWeek.Sunday)
            {
                switch (sortingDate.DayOfWeek)
                {
                    case DayOfWeek.Saturday:
                        sortingDate = sortingDate.AddDays(2);
                        sortingDate = new DateTime(sortingDate.Year, sortingDate.Month, sortingDate.Day, workDayStart, 0, 0);
                        break;
                    case DayOfWeek.Sunday:
                        sortingDate = sortingDate.AddDays(1);
                        sortingDate = new DateTime(sortingDate.Year, sortingDate.Month, sortingDate.Day, workDayStart, 0, 0);
                        break;
                }
            }

            else
            {
                if (sortingDate.Hour < workDayStart)
                {
                    sortingDate = new DateTime(sortingDate.Year, sortingDate.Month, sortingDate.Day, workDayStart, 0, 0);
                }


                if (sortingDate.Hour >= workDayStart && sortingDate.Hour < workDayEnd)
                {
                    availableWorkingMinutesOnCurrentWorkingDay = ((workDayEnd - sortingDate.Hour) * 60);
                    if (sortingDate.Minute > 0) { availableWorkingMinutesOnCurrentWorkingDay = availableWorkingMinutesOnCurrentWorkingDay - sortingDate.Minute; }

                    if (availableWorkingMinutesOnCurrentWorkingDay - minutesRequiredToFinishTask >= 0)
                    {
                        sortingDate = new DateTime(sortingDate.Year, sortingDate.Month, sortingDate.Day, (sortingDate.Hour + minutesRequiredToFinishTask / 60), (sortingDate.Minute + minutesRequiredToFinishTask % 60), 0);
                        minutesRequiredToFinishTask = 0;
                    }

                    else if (availableWorkingMinutesOnCurrentWorkingDay - minutesRequiredToFinishTask < 0)
                    {
                        minutesRequiredToFinishTask = minutesRequiredToFinishTask - availableWorkingMinutesOnCurrentWorkingDay;
                        sortingDate = sortingDate.AddDays(1);
                        sortingDate = new DateTime(sortingDate.Year, sortingDate.Month, sortingDate.Day, workDayStart, 0, 0);
                    }
                }


                else if (sortingDate.Hour > workDayEnd)
                {
                    sortingDate = new DateTime(sortingDate.Year, sortingDate.Month, sortingDate.Day + 1, workDayStart, 0, 0);
                }
            }
        }

        return sortingDate;

    }




	// Use this for initialization
	void Start () {

        if (TaskAssigner(new DateTime(2018, 6, 4, 10, 0, 0), 5) == new DateTime(2018, 6, 4, 15, 0, 0))
        {
            Debug.Log("CORRECT: Task received during workday, can be finished same day");
        }
        else
        {
            Debug.Log("WRONG: Task received during workday, can be finished same day");
        }


        if (TaskAssigner(new DateTime(2018, 6, 4, 10, 0, 0), 10) == new DateTime(2018, 6, 5, 12, 0, 0))
        {
            Debug.Log("CORRECT: Task received during workday, can NOT be finished same day");
        }
        else
        {
            Debug.Log("WRONG: Task received during workday, can NOT be finished same day");
        }


        if (TaskAssigner(new DateTime(2018, 6, 4, 7, 0, 0), 4) == new DateTime(2018, 6, 4, 13, 0, 0))
        {
            Debug.Log("CORRECT: Task received during workday BEFORE working hours, can be finished same day");
        }
        else
        {
            Debug.Log("WRONG: Task received during workday BEFORE working hours, can be finished same day");
        }


        if (TaskAssigner(new DateTime(2018, 6, 4, 18, 0, 0), 7) == new DateTime(2018, 6, 5, 16, 0, 0))
        {
            Debug.Log("CORRECT: Task received during workday AFTER working hours, can be finished same day");
        }
        else
        {
            Debug.Log("WRONG: Task received during workday AFTER working hours, can be finished same day");
        }


        if (TaskAssigner(new DateTime(2018, 6, 16, 11, 0, 0), 2) == new DateTime(2018, 6, 18, 11, 0, 0))
        {
            Debug.Log("CORRECT: Task received during weekend, can be finished same day");
        }
        else
        {
            Debug.Log("WRONG: Task received during weekend, can be finished same day");
        }


        if (TaskAssigner(new DateTime(2018, 12, 31, 10, 0, 0), 18) == new DateTime(2019, 1, 2, 12, 0, 0))
        {
            Debug.Log("CORRECT: Task received during workdays, can only be completed next year, two days from receive date");
        }
        else
        {
            Debug.Log("WRONG: Task received during workdays, can only be completed next year, two days from receive date");
        }


        if (TaskAssigner(new DateTime(2018, 6, 4, 10, 36, 0), 5) == new DateTime(2018, 6, 4, 15, 36, 0))
        {
            Debug.Log("CORRECT: Task received during workdays, can only be completed next year, two days from receive date");
        }
        else
        {
            Debug.Log("WRONG: Task received during workdays, can only be completed next year, two days from receive date");
        }

    }

}
