using System;

public class EffectComponent : GameEntityComponent
{
    private int m_EffectHandles;

    private int EffectHandles
    {
        get => m_EffectHandles;
        set
        {
            m_EffectHandles = value;
            enabled = m_EffectHandles > 0;
        }
    }

    private void Awake()
    {
        EffectHandles = 0;
    }

    private readonly struct EffectHandle : IDisposable
    {
        private readonly EffectComponent m_Component;

        public EffectHandle(EffectComponent component)
        {
            m_Component = component;
        }

        public void Dispose()
        {
            m_Component.EffectHandles--;
        }
    }

    public IDisposable GetEffectHandle()
    {
        EffectHandles++;
        return new EffectHandle(this);
    }
}