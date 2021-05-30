namespace Common.DateTime
{
    public interface IDateTimeProvider
    {
        System.DateTime UtcNow { get; }
    }
}
