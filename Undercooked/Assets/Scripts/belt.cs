public class Belt : Stanica
{
    public override void ChildUpdate()
    {
        if (hasInput)
        {
            hasInput = false;
            mj.VratCenuJedla(input);
        }
        hasOutput = false;
        free = true;
    }
}