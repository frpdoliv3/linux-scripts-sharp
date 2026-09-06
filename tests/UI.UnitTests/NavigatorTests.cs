using AwesomeAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace UI.UnitTests;

public class NavigatorTests : IAsyncDisposable
{
    private class FakeScreenA : IScreen
    {
        public Task ShowAsync()
        {
            throw new NotImplementedException();
        }
    }

    private class FakeScreenB : IScreen
    {
        public Task ShowAsync()
        {
            throw new NotImplementedException();
        }
    }

    private readonly INavigator _navigator;
    
    public NavigatorTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<FakeScreenA>();
        serviceCollection.AddScoped<FakeScreenB>();

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
        _navigator = new Navigator(scopeFactory);
    }

    [Fact]
    public void MustStartEmpty()
    {
        _navigator.CurrentScreen.Should().BeNull();
    }

    [Fact]
    public async Task MustContainStart()
    {
        _navigator.Push(ScreenType.Of<FakeScreenA>());
        _navigator.CurrentScreen.Should().BeOfType<FakeScreenA>();
    }

    [Fact]
    public async Task MustLastPushBeOnTop()
    {
        _navigator.Push(ScreenType.Of<FakeScreenA>());
        _navigator.Push(ScreenType.Of<FakeScreenB>());

        _navigator.CurrentScreen.Should().BeOfType<FakeScreenB>();
    }
    
    [Fact]
    public async Task MustHoldPreviousScreen()
    {
        _navigator.Push(ScreenType.Of<FakeScreenA>());
        _navigator.Push(ScreenType.Of<FakeScreenB>());

        _navigator.CurrentScreen.Should().BeOfType<FakeScreenB>();

        await _navigator.PopAsync();
        
        _navigator.CurrentScreen.Should().BeOfType<FakeScreenA>();
    }
    
    [Fact]
    public async Task MustPushReplace()
    {
        _navigator.Push(ScreenType.Of<FakeScreenA>());
        await _navigator.PushReplacementAsync(ScreenType.Of<FakeScreenB>());

        _navigator.CurrentScreen.Should().BeOfType<FakeScreenB>();

        await _navigator.PopAsync();

        _navigator.CurrentScreen.Should().BeNull();
    }
    
    [Fact]
    public async Task MustReplaceRoot()
    {
        _navigator.Push(ScreenType.Of<FakeScreenA>());
        _navigator.CurrentScreen.Should().BeOfType<FakeScreenA>();
        await _navigator.ClearStack();
        _navigator.CurrentScreen.Should().BeNull();
    }

    [Fact]
    public async Task MustThrowOnEmptyStack()
    {
        var act = async () => await _navigator.PushReplacementAsync(ScreenType.Of<FakeScreenA>());
        await act.Should().ThrowAsync<InvalidOperationException>();
    }
    
    public async ValueTask DisposeAsync()
    {
        await _navigator.DisposeAsync();
    }
}