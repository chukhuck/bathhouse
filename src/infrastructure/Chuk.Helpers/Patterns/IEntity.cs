namespace Chuk.Helpers.Patterns
{
  public interface IEntity<TKey> where TKey : struct
  {
    TKey Id { get; set; }
  }
}
