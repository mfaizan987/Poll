namespace Poll.Application.Interfaces
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        string UserName { get; }
        Guid? WorkSpaceId { get; }
        Guid? StationId { get; }
        Guid? CompanyId { get; }
    }
}
    