public struct HPChange
{
    public CharacterBase target;
    public int updated;
    public HPChange(CharacterBase target, int amount) { this.target = target; this.updated = amount; }
}

public struct ShieldChange
{
    public CharacterBase target;
    public int updated;
    public ShieldChange(CharacterBase target, int amount) { this.target = target; this.updated = amount; }
}

public struct BuffChange
{
    public CharacterBase target;
    public BuffType buffType;
    public int updated;
    public BuffChange(CharacterBase target, BuffType buffType, int amount) { this.target = target; this.buffType = buffType; this.updated = amount; }
}