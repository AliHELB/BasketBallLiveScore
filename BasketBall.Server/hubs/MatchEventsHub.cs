using Microsoft.AspNetCore.SignalR;

public class MatchEventsHub : Hub
{
    public async Task NotifyBasketEvent(int matchId, object basketEvent)
    {
        await Clients.Group(matchId.ToString()).SendAsync("BasketEventAdded", basketEvent);
    }

    public async Task NotifyFaultEvent(int matchId, object faultEvent)
    {
        await Clients.Group(matchId.ToString()).SendAsync("FaultEventAdded", faultEvent);
    }

    public async Task NotifySubstitutionEvent(int matchId, object substitutionEvent)
    {
        await Clients.Group(matchId.ToString()).SendAsync("SubstitutionEventAdded", substitutionEvent);
    }

    public async Task JoinMatchGroup(int matchId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, matchId.ToString());
    }

    public async Task UpdateTimer(int matchId, int remainingTime)
    {
        await Clients.Group(matchId.ToString()).SendAsync("TimerUpdated", matchId, remainingTime);
    }

    public async Task StartTimer(int matchId)
    {
        await Clients.Group(matchId.ToString()).SendAsync("TimerStarted", matchId);
    }

    public async Task PauseTimer(int matchId)
    {
        await Clients.Group(matchId.ToString()).SendAsync("TimerPaused", matchId);
    }

    public async Task QuarterChanged(int matchId, int quarter)
    {
        await Clients.Group(matchId.ToString()).SendAsync("QuarterChanged", matchId, quarter);
    }
}
