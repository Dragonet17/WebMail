namespace EGrower.Infrastructure.Extension.JWT {
    public interface IJWTSettings {
        string Key { get; set; }
        int ExpiryDays { get; set; }
    }
}