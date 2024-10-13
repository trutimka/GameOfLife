public class TimerView : UIView
{
    private Timer _timer;
    
    protected override void Start()
    {
        base.Start();

        _timer = FindObjectOfType<Timer>();
        _timer.TimerChanged += UpdateView;
        
        UpdateView();
    }

    protected override void UpdateView()
    {
        base.UpdateView();

        _text.text = $"Timer: {_timer.TimeToEnd}s";
    }
}
