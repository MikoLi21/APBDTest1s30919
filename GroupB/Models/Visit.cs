﻿namespace GroupB.Models;
public class Visit
{
    public int VisitId { get; set; }
    public DateTime Date { get; set; }
    public int ClientId { get; set; }
    public int MechanicId { get; set; }
}
