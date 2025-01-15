namespace AppointmentsManager.Domain.Entities
{
    public class DateTimeWork
    {
        public int Id { get; set; }
        public int IdDoctor { get; set; }
        public int DayOfWeek { get; set; } // 1-7 (Seg-Sáb)
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public DateTimeWork() { }
        public DateTimeWork(int dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
        }
        public DateTimeWork(int idDoctor, int dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            IdDoctor = idDoctor;
            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
        }
        public string GetDayName()
        {
            return DayOfWeek switch
            {
                1 => "Domingo",
                2 => "Segunda-feira",
                3 => "Terça-feira",
                4 => "Quarta-feira",
                5 => "Quinta-feira",
                6 => "Sexta-feira",
                7 => "Sábado",
                _ => throw new ArgumentException("Dia da semana inválido.")
            };
        }
    }
}
