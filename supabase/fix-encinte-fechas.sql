-- Fix existing encinte records: update their "fecha" to match the week's start date
-- This ensures the "edad del racimo" calculation is accurate.

UPDATE harvest_records 
SET fecha = harvest_calendars."FechaInicio"
FROM harvest_calendars 
WHERE harvest_records.semana_encinte = harvest_calendars."Semana"
  AND harvest_records.ano_encinte = harvest_calendars."Ano";

-- Verify:
SELECT "Id", semana_encinte, ano_encinte, fecha 
FROM harvest_records 
ORDER BY semana_encinte;
