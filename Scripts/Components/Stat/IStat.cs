using AbilitySystem;

public interface IStat
{
    void Init(float value);
    float Add(float value);
    float Subtract(float value);
    float AddModifier(float value);
    float RemoveModifier(float value);
    float GetMaxValue();
    float GetValue();
}
