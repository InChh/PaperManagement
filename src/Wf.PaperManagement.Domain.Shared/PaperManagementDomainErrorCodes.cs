﻿namespace Wf.PaperManagement;

public static class PaperManagementDomainErrorCodes
{
    public const string CompleteTimeShouldLaterThanReceiveTime = "Pm.Error:CompleteTimeShouldLaterThanReceiveTime";
    public const string WorkerIdAlreadyExists = "Pm.Error:WorkerIdAlreadyExists";
    public const string TwoWorkersCannotBeTheSame = "Pm.Error:TwoWorkersCannotBeTheSame";
}