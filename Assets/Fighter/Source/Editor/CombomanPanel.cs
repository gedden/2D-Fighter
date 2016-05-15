using Comboman;

public abstract class CombomanPanel : ICombomanPanel
{
    public abstract void Draw();
    public abstract void OnCharacterLoaded(CharacterData data);

    /// <summary>
    /// Character accessor
    /// </summary>
    public CharacterData Character
    {
        get
        {
            return CombomanEditor.Instance.Character;
        }
        set
        {
            CombomanEditor.Instance.Character = value;
        }
    }
}
