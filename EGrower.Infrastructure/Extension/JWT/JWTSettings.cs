namespace EGrower.Infrastructure.Extension.JWT {
    public class JWTSettings:IJWTSettings {
        public string Key { get; set; }
        public int ExpiryDays { get; set; }
    }
}