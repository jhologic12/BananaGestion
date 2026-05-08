-- Fix existing encinte records: update their "fecha" to match the week's start date
-- This ensures the "edad del racimo" calculation is accurate

UPDATE harvest_records hr
SET fecha = hc."FechaInicio"
FROM harvest_calendars hc
WHERE hr."SemanaEncinte" = hc."Semana"
  AND hr."AnoEncinte" = hc."Ano";

-- Verify the update
-- SELECT id, "LoteId", "SemanaEncinte", "AnoEncinte", fecha 
-- FROM harvest_records 
-- ORDER BY "SemanaEncinte";
