-- Update all harvest_calendars records from 2027 to 2026
-- This fixes the mismatch between DB data and frontend queries

UPDATE harvest_calendars 
SET 
    ano = 2026,
    fecha_inicio = fecha_inicio - INTERVAL '1 year',
    fecha_fin = fecha_fin - INTERVAL '1 year'
WHERE ano = 2027;

-- Verify the update
-- SELECT semana, ano, color_nombre, fecha_inicio, fecha_fin 
-- FROM harvest_calendars 
-- WHERE ano = 2026 
-- ORDER BY semana;
