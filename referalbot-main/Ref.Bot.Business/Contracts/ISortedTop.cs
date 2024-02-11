namespace Ref.Bot.Business.Contracts
{
	public interface ISortedTop
	{
		Task SortUsersForTop(long tgid);
	}
}
