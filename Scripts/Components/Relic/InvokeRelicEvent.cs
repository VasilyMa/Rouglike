namespace Client 
{
    struct InvokeRelicEvent
    {
        EffectAmplifier EffectAmplifier;

        public void SetEffect(EffectAmplifier effectAmplifier)
        {
            EffectAmplifier = effectAmplifier;
        }

        public void Resolve()
        {
            EffectAmplifier.ResolveEffect();
        }
    }
}