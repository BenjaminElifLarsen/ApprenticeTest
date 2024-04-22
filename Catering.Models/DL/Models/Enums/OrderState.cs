namespace Catering.Shared.DL.Models.Enums;

public enum OrderState
{
    Received = 1,
    Preparing = 2,
    ReadyToDeliver = 3,
    Delivered = 4,
    FailedToBeDelivered = 5,

    Unknown = 0,
}