-- Fix existing encinte records: update their "fecha" to match the week's start date
-- This ensures the "edad del racimo" calculation is accurate

UPDATE harvest_records hr
SET fecha = hc."fecha_inicio"
FROM harvest_calendars hc
WHERE hr.semana_encinte = hc.semana
  AND hr.ano_encinte = hc.ano;

-- Verify the update
-- SELECT id, semana_encinte, ano_encinte, fecha 
-- FROM harvest_records 
-- ORDER BY semana_encinte;

