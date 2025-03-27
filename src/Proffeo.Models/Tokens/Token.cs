namespace Proffeo.Models.Tokens;

public class Token
{
    public string Value { get; private set; }
    public DateTime Expires { get; private set; }

    private Token(string value, DateTime expires)
    {
        Value = value;
        Expires = expires;
    }
    
    public static Token Create(string value, DateTime expires) => new(value, expires);
}