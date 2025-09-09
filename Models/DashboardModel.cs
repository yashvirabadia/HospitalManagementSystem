namespace HospitalManagementSystem.Models
{
    public class DashboardModel
    {
        // Entity counts
        public int UserCount { get; set; }
        public int DoctorCount { get; set; }
        public int PatientCount { get; set; }
        public int DepartmentCount { get; set; }
        public int AppointmentCount { get; set; }
        public int DocDepCount { get; set; }

        // Revenue (per month)
        public List<string> RevenueLabels { get; set; } = new();
        public List<decimal> RevenueData { get; set; } = new();

        // Appointments (per month)
        public List<string> AppointmentLabels { get; set; } = new();
        public List<int> AppointmentData { get; set; } = new();

        // 🔹 Top 5 Doctors by Appointments
        public List<string> TopDoctorLabels { get; set; } = new();
        public List<int> TopDoctorData { get; set; } = new();

        // 🔹 Appointment Status Distribution (Scheduled, Completed, Cancelled)
        public List<string> AppointmentStatusLabels { get; set; } = new();
        public List<int> AppointmentStatusData { get; set; } = new();
    }
}
