-- Seed HarvestCalendar for year 2026
-- Week cycle: Verde → Amarillo → Blanco → Azul → Rojo → Café → Negro → Naranja (8 weeks)
-- Weeks start on Monday

-- First, remove existing data for 2026 (idempotent)
DELETE FROM harvest_calendars WHERE ano = 2026;

-- Insert 52 weeks for 2026
INSERT INTO harvest_calendars (id, semana, ano, color_cinta, color_nombre, fecha_inicio, fecha_fin, activo) VALUES
-- Week 1-8: Cycle 1
(uuid_generate_v4(), 1, 2026, '#00FF00', 'Verde', '2026-01-05', '2026-01-11', true),
(uuid_generate_v4(), 2, 2026, '#FFFF00', 'Amarillo', '2026-01-12', '2026-01-18', true),
(uuid_generate_v4(), 3, 2026, '#FFFFFF', 'Blanco', '2026-01-19', '2026-01-25', true),
(uuid_generate_v4(), 4, 2026, '#0000FF', 'Azul', '2026-01-26', '2026-02-01', true),
(uuid_generate_v4(), 5, 2026, '#FF0000', 'Rojo', '2026-02-02', '2026-02-08', true),
(uuid_generate_v4(), 6, 2026, '#8B4513', 'Café', '2026-02-09', '2026-02-15', true),
(uuid_generate_v4(), 7, 2026, '#000000', 'Negro', '2026-02-16', '2026-02-22', true),
(uuid_generate_v4(), 8, 2026, '#FFA500', 'Naranja', '2026-02-23', '2026-03-01', true),
-- Week 9-16: Cycle 2
(uuid_generate_v4(), 9, 2026, '#00FF00', 'Verde', '2026-03-02', '2026-03-08', true),
(uuid_generate_v4(), 10, 2026, '#FFFF00', 'Amarillo', '2026-03-09', '2026-03-15', true),
(uuid_generate_v4(), 11, 2026, '#FFFFFF', 'Blanco', '2026-03-16', '2026-03-22', true),
(uuid_generate_v4(), 12, 2026, '#0000FF', 'Azul', '2026-03-23', '2026-03-29', true),
(uuid_generate_v4(), 13, 2026, '#FF0000', 'Rojo', '2026-03-30', '2026-04-05', true),
(uuid_generate_v4(), 14, 2026, '#8B4513', 'Café', '2026-04-06', '2026-04-12', true),
(uuid_generate_v4(), 15, 2026, '#000000', 'Negro', '2026-04-13', '2026-04-19', true),
(uuid_generate_v4(), 16, 2026, '#FFA500', 'Naranja', '2026-04-20', '2026-04-26', true),
-- Week 17-24: Cycle 3
(uuid_generate_v4(), 17, 2026, '#00FF00', 'Verde', '2026-04-27', '2026-05-03', true),
(uuid_generate_v4(), 18, 2026, '#FFFF00', 'Amarillo', '2026-05-04', '2026-05-10', true),
(uuid_generate_v4(), 19, 2026, '#FFFFFF', 'Blanco', '2026-05-11', '2026-05-17', true),
(uuid_generate_v4(), 20, 2026, '#0000FF', 'Azul', '2026-05-18', '2026-05-24', true),
(uuid_generate_v4(), 21, 2026, '#FF0000', 'Rojo', '2026-05-25', '2026-05-31', true),
(uuid_generate_v4(), 22, 2026, '#8B4513', 'Café', '2026-06-01', '2026-06-07', true),
(uuid_generate_v4(), 23, 2026, '#000000', 'Negro', '2026-06-08', '2026-06-14', true),
(uuid_generate_v4(), 24, 2026, '#FFA500', 'Naranja', '2026-06-15', '2026-06-21', true),
-- Week 25-32: Cycle 4
(uuid_generate_v4(), 25, 2026, '#00FF00', 'Verde', '2026-06-22', '2026-06-28', true),
(uuid_generate_v4(), 26, 2026, '#FFFF00', 'Amarillo', '2026-06-29', '2026-07-05', true),
(uuid_generate_v4(), 27, 2026, '#FFFFFF', 'Blanco', '2026-07-06', '2026-07-12', true),
(uuid_generate_v4(), 28, 2026, '#0000FF', 'Azul', '2026-07-13', '2026-07-19', true),
(uuid_generate_v4(), 29, 2026, '#FF0000', 'Rojo', '2026-07-20', '2026-07-26', true),
(uuid_generate_v4(), 30, 2026, '#8B4513', 'Café', '2026-07-27', '2026-08-02', true),
(uuid_generate_v4(), 31, 2026, '#000000', 'Negro', '2026-08-03', '2026-08-09', true),
(uuid_generate_v4(), 32, 2026, '#FFA500', 'Naranja', '2026-08-10', '2026-08-16', true),
-- Week 33-40: Cycle 5
(uuid_generate_v4(), 33, 2026, '#00FF00', 'Verde', '2026-08-17', '2026-08-23', true),
(uuid_generate_v4(), 34, 2026, '#FFFF00', 'Amarillo', '2026-08-24', '2026-08-30', true),
(uuid_generate_v4(), 35, 2026, '#FFFFFF', 'Blanco', '2026-08-31', '2026-09-06', true),
(uuid_generate_v4(), 36, 2026, '#0000FF', 'Azul', '2026-09-07', '2026-09-13', true),
(uuid_generate_v4(), 37, 2026, '#FF0000', 'Rojo', '2026-09-14', '2026-09-20', true),
(uuid_generate_v4(), 38, 2026, '#8B4513', 'Café', '2026-09-21', '2026-09-27', true),
(uuid_generate_v4(), 39, 2026, '#000000', 'Negro', '2026-09-28', '2026-10-04', true),
(uuid_generate_v4(), 40, 2026, '#FFA500', 'Naranja', '2026-10-05', '2026-10-11', true),
-- Week 41-48: Cycle 6
(uuid_generate_v4(), 41, 2026, '#00FF00', 'Verde', '2026-10-12', '2026-10-18', true),
(uuid_generate_v4(), 42, 2026, '#FFFF00', 'Amarillo', '2026-10-19', '2026-10-25', true),
(uuid_generate_v4(), 43, 2026, '#FFFFFF', 'Blanco', '2026-10-26', '2026-11-01', true),
(uuid_generate_v4(), 44, 2026, '#0000FF', 'Azul', '2026-11-02', '2026-11-08', true),
(uuid_generate_v4(), 45, 2026, '#FF0000', 'Rojo', '2026-11-09', '2026-11-15', true),
(uuid_generate_v4(), 46, 2026, '#8B4513', 'Café', '2026-11-16', '2026-11-22', true),
(uuid_generate_v4(), 47, 2026, '#000000', 'Negro', '2026-11-23', '2026-11-29', true),
(uuid_generate_v4(), 48, 2026, '#FFA500', 'Naranja', '2026-11-30', '2026-12-06', true),
-- Week 49-52: Cycle 7 (partial)
(uuid_generate_v4(), 49, 2026, '#00FF00', 'Verde', '2026-12-07', '2026-12-13', true),
(uuid_generate_v4(), 50, 2026, '#FFFF00', 'Amarillo', '2026-12-14', '2026-12-20', true),
(uuid_generate_v4(), 51, 2026, '#FFFFFF', 'Blanco', '2026-12-21', '2026-12-27', true),
(uuid_generate_v4(), 52, 2026, '#0000FF', 'Azul', '2026-12-28', '2026-12-31', true);

-- Verify the data
-- SELECT semana, color_nombre, fecha_inicio, fecha_fin FROM harvest_calendars WHERE ano = 2026 ORDER BY semana;
