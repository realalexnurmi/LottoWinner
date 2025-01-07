﻿-- Скрипт генерации текстовки для начальной вставки для 2-9ых столбцов
-- Так как предполагаемое количество билетов равно 33, а в каждом билете два поля со столбцами, то каждый номер столбца должен иметь 66 вариантов.
DECLARE @ColNum int = 2
DECLARE @FNum int
DECLARE @SNum int
DECLARE @EndNum int
DECLARE @Count int = 0
DECLARE @SingleCount int = 0
DECLARE @output varchar(max) = ''

DECLARE @t TABLE (F int, S int)

--"Особые" компромиссные средние значения для 2-8 столбцов - дополнительные 15 столбцов.
INSERT INTO @t (F, S)
VALUES
(10, 11),
(20, 21),
(30, 31),
(40, 41),
(50, 51),
(60, 61),
(70, 71),
(13, 17),
(23, 27),
(33, 37),
(43, 47),
(53, 57),
(63, 67),
(73, 77),
(40, 47)
--/*--(NEWID(), 1, 1, NULL, 7),
WHILE (@ColNum <= 9)
BEGIN
	SET @FNum = (@ColNum - 1) * 10
	SET @EndNum = (@ColNum - 1) * 10 + 10
	
	IF (@ColNum = 9)
	BEGIN
		-- Для последнего девятого столбца используются Сочетания из 11 по 2 = 55
		-- Для столбцов вида N, N+1 берем удвоенное количество и дополнительно столбец 80,90 (итого: 11)
		WHILE (@FNum < @EndNum)
		BEGIN
			SET @SNum = @FNum + 1
			WHILE (@SNum <= @EndNum)
			BEGIN
				SET @output = @output + '(NEWID(), '+ CAST(@ColNum as varchar(1)) + ', ' + CAST(@FNum as varchar(2)) + ', ' + CAST(@SNum as varchar(2)) + ', ' + IIF(@FNum + 1 = @SNum OR @FNum + 10 = @SNum, '2', '1') + '),
'
				SET @SNum += 1
			END
			SET @FNum += 1
		END
	END
	ELSE
	BEGIN
		-- Столбцы 2-7 по сформированным правилам должны содержать 2 столбца с одним числом.
		-- Число сочетаний из 7 по 2 = 21 (каждое число будет 6 раз) - что приводит к равномерному распределению столбцов с одним числом =>
		-- Каждый столбец будет равномерно участвовать в 18 из 63 комбинаций как столбец с одним числом.
		-- Оставшие 3 из 66 комбинаций - это дополнительные 6 вариантов одинарных столбцов.
		-- Каждое число из столбца представленое как одинарный столбец дает 10 вариантов (без кратного 10 = 9 вариантов)
		-- Так дополнительные 6 вариантов необходимо распределить по 7 столбцам - исключили один одинарный вариант из 5 столбца (Вида 4N - им станет 40).
		-- Итого: Для каждого столбца, кроме 5 должны иметь по 19 вариантов одинарных столбцов - ими становятся каждое число столбца, кроме кратного 10 дважды = 18 + единожды столбец кратный 10, за исключением 5-ого столбца.
		-- Для компенсации неравенства распределения, чутка сглаживается использование особых двойных столбцов (двойное использование числа 40), исользование по разу чисел кратных 10.
		-- По разу больше от среднего используются числа оканчивающиеся на 1, 3, 7. Число 47 от двойного использования в особых столбцах получается "выброс" на 1 в частоте в сторону увеличения.
		WHILE (@FNum < @EndNum)
		BEGIN
			SET @SNum = @FNum + 1
			WHILE (@SNum <= @EndNum)
			BEGIN
				if (@SNum = @EndNum)
				BEGIN
					SET @output = @output + '(NEWID(), '+ CAST(@ColNum as varchar(1)) + ', ' + CAST(@FNum as varchar(2)) + ', NULL, ' + IIF(@FNum % 10 = 0, IIF(@FNum = 40, '0', '1'), '2') + '),
'
				END
				ELSE
				BEGIN
					SET @output = @output + '(NEWID(), ' + CAST(@ColNum as varchar(1)) + ', ' + CAST(@FNum as varchar(2)) + ', ' + CAST(@SNum as varchar(2)) + ', ' + IIF(EXISTS(SELECT 1 FROM @t WHERE @FNum = F AND @SNum = S), '2', '1') + '),
'
				END
				SET @SNum += 1
			END
			SET @FNum += 1
		END
	END
	SET @ColNum += 1
	print @output
	SET @output = ''
END