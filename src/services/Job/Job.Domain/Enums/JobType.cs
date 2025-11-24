namespace Job.Domain.Enums;

public enum JobType
{
    Yerleştirme, // Mal kabul ---> Raf
    Toplama,      // Raf ---> Sevkiyat
    Transfer,     // Raf ---> Raf
    Sayım       // Raf
}