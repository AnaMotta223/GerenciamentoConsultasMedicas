namespace AppointmentsManager.Domain.Entities
{
    public class DateTimeWork
    {
        public Doctor Doctor { get; set; }
        public int DayOfWeek { get; set; } // 1-7 (Seg-Sáb)
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public DateTimeWork() { }
        public DateTimeWork(int dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            if (dayOfWeek < 1 || dayOfWeek > 7)
                throw new ArgumentException("O dia da semana deve estar entre 1 e 7.");

            if (startTime >= endTime)
                throw new ArgumentException("O horário de início deve ser anterior ao horário de fim.");

            DayOfWeek = dayOfWeek;
            StartTime = startTime;
            EndTime = endTime;
        }
        public DateTimeWork(Doctor doctor, int dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            if (dayOfWeek < 1 || dayOfWeek > 7)
                throw new ArgumentException("O dia da semana deve estar entre 1 e 7.");

            if (startTime >= endTime)
                throw new ArgumentException("O horário de início deve ser anterior ao horário de fim.");

            Doctor = doctor;
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
        public string GetFormattedStartTime()
        {
            return StartTime.ToString(@"hh\:mm");
        }
        public string GetFormattedEndTime()
        {
            return EndTime.ToString(@"hh\:mm");
        }
    }
}
