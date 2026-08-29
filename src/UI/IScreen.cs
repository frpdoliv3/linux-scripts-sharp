namespace UI;

public interface IScreen
{
    Task<Type?> ShowAsync();
}
