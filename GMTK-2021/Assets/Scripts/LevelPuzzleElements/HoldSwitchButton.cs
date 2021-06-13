namespace LogicalElements
{
    public class HoldSwitchButton : SwitchButton
    {
        protected override void Update()
        {
            if (!_isTouched)
                return;
            
            if (_touching.JustPressedUse || _touching.JustUnpressedUse)
                Switch();
        }
    }
}