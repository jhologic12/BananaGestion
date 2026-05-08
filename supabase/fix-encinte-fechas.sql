-- Fix existing encinte records: update their "fecha" to match the week's start date
-- This ensures the "edad del racimo" calculation is accurate

UPDATE harvest_records 
SET fecha = hc."FechaInicio"
FROM harvest_calendars hc
WHERE harvest_records."SemanaEncinte" = hc."Semana"
  AND harvest_records."AnoEncinte" = hc."Ano";

-- Verify the update
-- SELECT id, "LoteId", "SemanaEncinte", "AnoEncinte", fecha 
-- FROM harvest_records 
-- ORDER BY "SemanaEncinte";
