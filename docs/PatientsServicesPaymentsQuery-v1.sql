use MyPASDBProd;

select *
from Patients
left join Services
on Services.PatientId = Patients.id
left join Payments
on Payments.PatientId = Patients.id
;