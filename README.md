1. Перейшовши на сторінку "Завантаження файлу" можна імпортувати дані графіків вимкнення світла.
2. Валідації формату файлу присутні. 
3. Повторний запис даних перезаписується (див. попередження на сайті - графіки однієї черги обʼєднуються, якщо є часовий перетин)
4. На головній сторінці відображається, у кого немає світла на даний момент, і є загальний перегляд черг.
5. У хедері відображається поле пошуку, по якому можна відфільтрувати вимкнення лише на свою чергу.
6. Кожен інтервал вимкнення можна редагувати, можна додавати новий.
7. На головній сторінці можна експортувати json відображуваних даних. Якщо дані будуть відфільтровані - завантажаться відфільтровані.
8. При реалізації використовувався Asp.Net Core MVC

Відібрати адреси, яким не назначено групу:
SELECT * from "Address" WHERE "GroupId" is null

Відібрати графік відключень світла для адреси Бойченко 30:
SELECT s."Day", s."StartTime", s."FinishTime" from "Address" a
JOIN "Schedule" s ON a."GroupId" = s."GroupId"
WHERE a."Address" = 'Бойченко 30';

Відібрати групу, якій найчастіше виключають світло в неділю:
SELECT g."Id", g."Name", COUNT(*) AS "Frequency" from "Schedule" s
JOIN "Group" g ON s."GroupId" = g."Id" WHERE s."Day" = 'Неділя'
GROUP BY g."Name"
ORDER BY "Frequency" DESC
LIMIT 1;

Відібрати групу, якій вимикають світло на найбільший час з понеділка по середу включно:
SELECT g."Name", SUM(TIMESTAMPDIFF(HOUR, s."StartTime", s."FinishTime")) AS "TotalHours" from "Schedule" s
JOIN "Group" g ON s."GroupId" = g."Id" WHERE s."Day" IN ('Понеділок', 'Вівторок', 'Середа')
GROUP BY g."Name"
ORDER BY "TotalHours" DESC
LIMIT 1;

Назначити на адресу Бажана 14 групу 4:
UPDATE "Address" SET "GroupId" = 4 WHERE "Address" = 'Бажана 14';

Додати 2 нові адреси будь-які:
INSERT INTO "Address" ("Address", "GroupId")
VALUES ('Соборна 22', 1), ('Перемоги 1', NULL);

Додати новий графік вимкнення будь-який:
INSERT INTO "Schedule" ("Day", "StartTime", "FinishTime", "GroupId")
VALUES ('Четвер', '10:00', '12:00', 1);
