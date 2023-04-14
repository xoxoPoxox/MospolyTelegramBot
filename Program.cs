using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MpuAbiturienBot
{
    internal class Program
    {
        static Dictionary<int, string> UnifiedStateExam = new Dictionary<int, string>
        {
            {0,"русский язык"},
            {1, "математика"},
            {2,"физика"},
            {3,"химия"},
            {4,"история"},
            {5,"обществознание"},
            {6,"информатика(ИКТ)"},
            {7,"биология"},
            {8,"иностранный язык"},
            {9,"литература"},
            {10,"сбросить" },
            {11,"отмена" },
            {12,"готово" }
        };

        static Dictionary<int, string> Facults = new Dictionary<int, string>
        {
            {20, "Факультет графики и искусства книги имени В.А.Фаворского"},
            {21,"Факультет издательского дела и журналистики" },
            {22,"Факультет информационных технологий" },
            {23, "Факультет машиностроения" },
            {24, "Полиграфический факультет" },
            {25, "Транспортный факультет" },
            {26, "Факультет урбанистики и городского хозяйства" },
            {27, "Факультет химической технологии и биотехнологии" },
            {28, "Факультет экономики и управления" }
        };

        static Dictionary<int, string> StandartStateExam = new Dictionary<int, string>
        {
            {0,"русский язык"},
            {1, "математика"},
            {2,"физика"},
            {3,"химия"},
            {4,"история"},
            {5,"обществознание"},
            {6,"информатика(ИКТ)"},
            {7,"биология"},
            {8,"иностранный язык"},
            {9,"литература"},
            {10,"сбросить" },
            {11,"отмена" },
            {12,"готово" }
        };

        static Dictionary<int, string> FacultIT = new Dictionary<int, string>
        {
            {0,"\U0001F47EСистемная и программная инженерия\n Новая специальность\n Бюджетных мест: 30\n" + "\n" +
                "\U0001F47EИнформационные технологии управления бизнесом\n Новая специальность\n Бюджетных мест: 25" },
            {230, "\U0001F47EИнформационная безопасность\n Проходной балл: 230 \U00002705 \n Бюджетных мест: 48" },
            {232,"\U0001F47EЦифровая трансформация\n Проходной балл: 232\U00002705 \n Бюджетных мест: 25" },
            {234,"\U0001F47EКиберфизические системы\n Проходной балл: 234\U00002705 \n Бюджетных мест: 50" },
            {239,"\U0001F47EИнтеграция и программирование в САПР\n Проходной балл: 239 \U00002705 \n Бюджетных мест: 75" },
            {243,"\U0001F47EАвтоматизированные системы обработки информации и управления\n Проходной балл: 243 \U00002705 \n Бюджетных мест: 223\n" + "\n" +
                "\U0001F47EИнформационные технологии в медиандустрии и дизайне\n Проходной балл: 243 \U00002705 \n Бюджетных мест: 223\n" + "\n" +
                "\U0001F47EТехнологии дополненной и виртуальной реальности\n Проходной балл: 243 \U00002705 \n Бюджетных мест: 223\n" + "\n" +
                "\U0001F47EПрограммное обеспечение игровой компьютерной индустрии\n Проходной балл: 243 \U00002705 \n Бюджетных мест: 223\n" + "\n" +
                "\U0001F47EИнформационные системы умных пространств\n Проходной балл: 243 \U00002705 \n Бюджетных мест: 223" },
            {244,"\U0001F47EИнформационные системы и технологии обработки цифрового контента\n Проходной балл: 244 \U00002705 \n Бюджетных мест: 24\n Форма обучения: заочная" },
            {247,"\U0001F47EКорпоративные информационные системы\n Проходной балл: 247 \U00002705 \n Бюджетных мест: 50" },
            {248, "\U0001F47EИнформационная безопасность автоматизированных систем\n Проходной балл: 248 \U00002705 \n Бюджетных мест: 35" },
            {253,"\U0001F47EБольшие и открытые данные\n Проходной балл: 253 \U00002705 \n Бюджетных мест: 50" + "\n\n" +
                "\U0001F47EПрограммное обеспечение информационных систем\n Бюджетных мест: 23\n Проходной балл: 253 \U00002705 \n Форма обучения: заочная"},
            {263,"\U0001F47EВеб-технологии\n Проходной балл: 263 \U00002705 \n Бюджетных мест: 50"}
        };
        static Dictionary<int, string> FacultUrbanInfo = new Dictionary<int, string> // Специальности с информатикой/физикой
        {
            {177,"\U0001F4A1Автоматизированные энергетические установки\n Проходной балл: 177 \U00002705 \n Бюджетных мест: 25" },
            {186,"\U0001F4A1Теплоэнергетика и теплотехника\n Проходной балл: 186 \U00002705 \n Бюджетных мест: 25" },
            {191,"\U0001F4DEСистемы дальней связи\n Проходной балл: 191 \U00002705 \n Бюджетных мест: 25" },
            {208,"\U0001F3E2Промышленное и гражданское строительство\n Проходной балл: 208 \U00002705 \n Бюджетных мест: 57" },
            {210,"\U0001F4A1Электроэнергетика и электротехника\n Проходной балл: 210 \U00002705 \n Бюджетных мест: 25" },
            {215,"\U0001F3E2Строительство уникальных зданий и сооружений\n Проходной балл: 215 \U00002705 \n Бюджетных мест: 30" },
            {226,"\U0001F4A1Теплоэнергетика и теплотехника\n Проходной балл: 226 \U00002705 \n Бюджетных мест: 24\n Форма обучения: заочная" },
            {233,"\U0001F4A1Электроэнергетика и электротехника\n Проходной балл: 233 \U00002705 \n Бюджетных мест: 24\n Форма обучения: заочная" }
        };

        static Dictionary<int, string> FacultUrbanPhysic = new Dictionary<int, string> // Специальности только с физикой
        {
            {216, "\U0001F304Шахтное и подземное строительство\n Проходной балл: 216 \U00002705 \n Бюджетных мест: 216\n Форма обучения: заочная" +"\n\n" +
                "\U0001F304Маркшейдерское дело\n Проходной балл: 216 \U00002705 \n Бюджетных мест: 216\n Форма обучения: заочная"}
        };

        static Dictionary<int, string> FacultBiotechIT = new Dictionary<int, string> // Только с инфой/физикой
        {
            {0,"\U0001F4BBАвтоматизированное проектирование технологических процессов и производств\n Новая специальность\n Бюджетных мест: 20" },
        };


        static Dictionary<int, string> FacultBiotechBio = new Dictionary<int, string> // Только с био
        {
            {237,"\U0001F4BBБиотехнология\n Проходной балл: 237 \U00002705 \n Бюджетных мест: 33" }
        };

        static Dictionary<int, string> FacultBiotechOther = new Dictionary<int, string>
        {
            {152,"\U0001F4DAЭкологическая безопасность и охрана труда\n Проходной балл: 152 \U00002705 \n Бюджетных мест: 55" },
            {175,"\U0001F4DAБезотходные производственные технологии\n Проходной балл: 175 \U00002705 \n Бюджетных мест: 25" },
        };

        static Dictionary<int, string> FacultPoligraficInfo = new Dictionary<int, string> // С информатикой или физикой
        {
            {0,"\U0001F4F1Цифровизация технологических процессов\n Новая специальность\n Бюджетных мест: 20\n Форма обучения: заочная" +"\n\n" +
                "\U0001F4F1Технология упаковочного производства\n \U00002757Новая форма обучения\U00002757\n Бюджетных мест: 25\n Форма обучения: очно-заочная" + "\n\n" +
                "\U0001F4C8Бизнес-процессы печатной и упаковочной индустрии\n \U00002757Новая форма обучения\U00002757\n Бюджетных мест: 25\n Форма обучения: очно-заочная" + "\n\n" +
                "\U0001F4F1Ресурсное обеспечение печатной и упаковочной индустрии\n\U00002757 Новая форма обучения\U00002757\n Бюджетных мест: 25\n Форма обучения: очно-заочная" + "\n\n" +
                "\U0001F3A8Дизайн и проектирование мультимедиа и визуального контента\n \U00002757Новая форма обучения\U00002757\n Бюджетных мест: 25\n Форма обучения: очно-заочная"},
            {170,"\U0001F4DDМатериаловедение и цифровые технологии\n Проходной балл: 170 \U00002705 \n Бюджетных мест: 19" },
            {192, "\U0001F4F1Управление качеством\n Проходной балл: 192 \U00002705 \n Бюджетных мест: 19" },
            {201,"\U0001F4F1Технология упаковочного производства\n Проходной балл: 201 \U00002705 \n Бюджетных мест: 85" + "\n\n" +
                "\U0001F4C8Бизнес-процессы печатной и упаковочной индустрии\n Проходной балл: 201 \U00002705 \n Бюджетных мест: 85" + "\n\n" +
                "\U0001F4F1Ресурсное обеспечение печатной и упаковочной индустрии\n Проходной балл: 201 \U00002705 \n Бюджетных мест: 85" + "\n\n" +
                "\U0001F3A8Дизайн и проектирование мультимедиа и визуального контента\n Проходной балл: 201 \U00002705 \n Бюджетных мест: 85"},
            {238,"\U0001F4F1Информационные системы автоматизированных комплексов медиаиндустрии\n Проходной балл: 238 \U00002705 \n Бюджетных мест: 25" }
        };

        static Dictionary<int, string> FacultPoligraficOther = new Dictionary<int, string> // Только с химией
        {
            {170,"\U0001F4F1Материаловедение и цифровые технологии\n Проходной балл: 170 \U00002705 \n Бюджетных мест: 19" }
        };

        static Dictionary<int, string> FacultEconomicIT = new Dictionary<int, string> // Инфа/физон
        {
            {0,"\U0001F4C8Информационные технологии управления бизнесом\n Новая специальность\n Бюджетных мест: 25" },
        };

        static Dictionary<int, string> FacultEconomicHistory = new Dictionary<int, string> // Только с историей/англом
        {
             {262, "\U0001F4ACРеклама и связи с общественностью в цифровых медиа\n Проходной балл: 262 \U00002705 \n Бюджетных мест: 14\n Форма обучения: заочная" },
            {282,"\U0001F4ACРеклама и связи с общественностью в цифровых медиа\n Проходной балл: 282 \U00002705 \n Бюджетных мест: 15" }
        };

        static Dictionary<int, string> FacultEconomicOther = new Dictionary<int, string>
        {
            {0, "\U0001F4B2Цифровая экономика и финансы предприятия\n Новая специальность\n Платных мест: 25" +"\n\n" +
                "\U0001F4B2Digital Economy and Finance of Enterprise (study in English)\n Новая специальность\n Платных мест: 25\n Форма обучения: очно-заочная" + "\n\n" +
                "\U0001F4C8Управление бизнес-процессами\n Новая специальность\n Платных мест: 25" +"\n\n" +
                "\U0001F4C8Business process Management (study in English)\n Новая специальность\n Платных мест: 25\n Форма обучения: очно-заочная" + "\n\n" +
                "\U0001F4B2Экономика и управление трудом\n Новая специальность\n Платных мест: 25\n Форма обучения: очно-заочная" + "\n\n" +
                "\U0001F4ACРеклама и связи с общественностью в цифровых медиа\n \U00002757Новая форма обучения\U00002757\n Платных мест: 30\n Форма обучения: очно-заочная"},
        };

        static Dictionary<int, string> FacultPublishingLiterature = new Dictionary<int, string>
        {
            {0,  "\U0001F4E0Периодические издания и мультимедийная журналистика, Деловая журналистика\n Бюджетных мест: 20 \n \U00002757Новая форма обучения\U00002757\n Форма обучения: заочная\n Вступительные испытания: Собеседование"},
            {298,"\U0001F4E0Периодические издания и мультимедийная журналистика, Деловая журналистика\n Бюджетных мест: 15 \n Проходной балл: 298 \U00002705 \n Вступительные испытания: Собеседование" },

        };

        static Dictionary<int, string> FacultPublishingOther = new Dictionary<int, string>
        {
            {249, "\U0001F3A8Книгоиздательское дело; Газетно-журнальное издательское дело\n Бюджетных мест: 14\n Проходной балл: 249 \U00002705 \n Форма обучения: заочная" },
            {276, "\U0001F3A8Книгоиздательское дело; Газетно-журнальное издательское дело\n Бюджетных мест: 20\n Проходной балл: 276 \U00002705 " }
        };

        static Dictionary<int, string> FacultEngineeringIT = new Dictionary<int, string> // Направления в информатикой
        {
            {171,"\U0001F529Машины и технологии обработки материалов давлением\n Бюджетных мест: 66\n Проходной балл: 171 \U00002705 "+ "\n\n" +
                "\U0001F529Оборудование и технологии сварочного производства\n Бюджетных мест: 66\n Проходной балл: 171 \U00002705 " + "\n\n" +
                "\U0001F529Высокоэффективные технологические процессы и оборудование\n Бюджетных мест: 66\n Проходной балл: 171 \U00002705 " + "\n\n" +
            "\U0001F529Комплексные технологические процессы и оборудование машиностроения\n Бюджетных мест: 20\n Проходной балл: 171 \U00002705 \n Форма обучения: очно-заочная"},
            {225, "\U0001F529Комплексные технологические процессы и оборудование машиностроения\n Бюджетных мест: 20\n Проходной балл: 171 \U00002705 \n Форма обучения: заочная"},
            {245,"\U0001F529Машины и технологии обработки материалов давлением\n Бюджетных мест: 47\n Проходной балл: 245 \U00002705 \n Форма обучения: заочная"+ "\n\n" +
                "\U0001F529Оборудование и технологии сварочного производства\n Бюджетных мест: 47\n Проходной балл: 245 \U00002705 \n Форма обучения: заочная" + "\n\n" +
                "\U0001F529Высокоэффективные технологические процессы и оборудование\n Бюджетных мест: 47\n Проходной балл: 245 \U00002705 \n Форму обучения: заочная"},
            {218, "\U0001F916Роботы и робототехнические устройства\n Бюджетных мест: 20\n Проходной балл: 218 \U00002705 "},
            {0, "\U000026A1Конструкторско-технологическое обеспечение цифрового производства\n Бюджетных мест: 20\n Новое направление"},
            {189, "\U000026A1Цифровая метрология\n Бюджетных мест: 19\n Проходной балл: 189 \U00002705" },
            {215, "\U000026A1Электронные системы управления\n Бюджетных мест: 19\n Проходной балл: 215 \U00002705" },
            {230, "\U0001F916Аддитивные технологии(Инноватика)\n Бюджетных мест: 19\n Проходной балл: 230 \U00002705" },
            {197, "\U0001F3A8Современные технологии в производстве художественных изделий\n Бюджетных мест: 50\n Проходной балл: 197 \U00002705 \n Вступительные: Рисунок геометрических фигур" + "\n\n" +
                "\U0001F3A8Художественное проектирование и цифровые технологии в ювелирном производстве\n Бюджетных мест: 50\n Проходной балл: 197 \U00002705 \n Вступительные: Рисунок геометрических фигур" }

        };
        static Dictionary<int, string> FacultEngineeringOther = new Dictionary<int, string>// С химией/физикой/инфой
        {
            {129, "\U0001F50DИнновации в металлургии\n Бюджетных мест: 23\n Проходной балл: 129 \U00002705 \n Форма обучения: очно-заочная" },
            {167,"\U0001F50DПерспективные материалы и технологии\n Бюджетных мест: 20 \n Проходной балл: 167 \U00002705 " },
            {218, "\U0001F50DИнновации в металлургии\n Бюджетных мест: 20\n Проходной балл: 218 \U00002705 \n Форма обучения: заочная" }
        };

        static Dictionary<int, string> FacultGraphic = new Dictionary<int, string>
        {
            {0,"\U0001F3A8Графический дизайн мультимедия\n Бюджетных мест: 20\n Проходной балл: 396 \U00002705 \n Вступительные: Рисунок и колористика, Декоративно-шрифтовая композиция с геометрическими элементами" + "\n\n" +
                "\U0001F3A8Художник анимации и компьютерной графики\n Бюджетных мест: 65\n Проходной балл: 348 \U00002705 \n Вступительные: Рисунок и живопись, Композиция-иллюстрация" + "\n\n" +
                "\U0001F3A8Художник-график [оформление печатной продукции]\n Бюджетных мест: 65\n Проходной балл: 348 \U00002705 \n Вступительные: Рисунок и живопись, Композиция-иллюстрация "},
        };

        static Dictionary<int, string> FacultTransportInfo = new Dictionary<int, string>
        {
            {0, "\U0001F697Перспективные транспортные средства\n Бюджетных мест: 25\n \U00002757Новая форма обучения\U00002757\n Форма обучения: заочная" + "\n\n" +
                "\U0001F697Автомобили и автомобильный сервис\n Бюджетных мест: 20\n Новое направление"},
            {185, "\U0001F916Компьютерный инжиниринг в автомобилестроении\n Бюджетных мест: 20\n Проходной балл: 185 \U00002705 " + "\n\n" +
                "\U0001F4A1Энергоустановки для транспорта и малой энергетики\n Бюджетных мест: 25\n Проходной балл: 185 \U00002705 "},
            {238, "\U0001F916Интеллектуальные системы управления транспортом\n(Прикладная математика и информатика)\n Бюджетных мест: 31\n Проходной балл: 238 \U00002705" },
            {234, "\U0001F697Спортивные транспортные средства\n Бюджетных мест: 20\n Проходной балл: 234 \U00002705" },
            {221, "\U0001F697Перспективные транспортные средства\n Бюджетных мест: 20\n Проходной балл: 221 \U00002705" },
            {217, "\U0001F4A1Энергоустановки для транспорта и малой энергетики\n Бюджетных мест: 20\n Проходной балл: 217 \U00002705 \n Форма обучения: заочная"},
            {212, "\U0001F916Программирование и цифровые технологии в динамике и прочности\n(Прикладная механика)\n Бюджетных мест: 20\n Проходной балл: 212\U00002705" },
            {164, "\U0001F697Инжиниринг и эксплуатация транспортных систем\n Бюджетных мест: 25\n Проходной балл: 164\U00002705\n Форма обучения: очно-заочная" },
            {247, "\U0001F697Инжиниринг и эксплуатация транспортных систем\n Бюджетных мест: 20\n Проходной балл: 247\U00002705\n Форма обучения: заочная" }
        };
        static Dictionary<int, string> FacultTransportOther = new Dictionary<int, string>
        {
          {0, "\U0001F3A8Транспортный и промышленный дизайн\n Бюджетных мест: 11\n Проходной балл: 292\U00002705\n Вступительные: Специальный рисунок, Академический рисунок" }
        };

        static int[] amount = new int[1];
        static int[] countlessons = new int[10]; // Считаем чтоб предметы не повторялись
        static int?[] select = new int?[3];
        static int[] summaballov = new int[1]; // Запоминаем кол-во баллов которое вводит пользователь
        static List<int> summapredmetov = new List<int>();
        static int idmessage_inline = 0; // id сообщения клавиатуры
        static bool message_balls = false;
        static void zero() // Функция обнуления счётчиков, убирает плюсики с выбранных предметов
        {
            amount[0] = 0;
            message_balls = false;
            summapredmetov = new List<int>();
            for (int i = 0; i < 10; i++)
            {
                UnifiedStateExam[i] = StandartStateExam[i]; // Убираем плюсики с массива
                countlessons[i] = 0;
            }
        }

        static InlineKeyboardMarkup InlineKeyboardUnifiedStateExam() // Функция генерации клавиатуры с выбором предметов
        {
            List<List<InlineKeyboardButton>> testTest = new List<List<InlineKeyboardButton>>();
            for (int i = 0; i < UnifiedStateExam.Count - 1; i += 2)
            {
                List<InlineKeyboardButton> test = new List<InlineKeyboardButton>();
                for (int j = i; j < i + 2; j++)
                    if (select[0] != j)
                        test.Add(InlineKeyboardButton.WithCallbackData(text: UnifiedStateExam[j], callbackData: $"{j}"));
                    else
                        test.Add(InlineKeyboardButton.WithCallbackData(text: $"{UnifiedStateExam[j]}", callbackData: $"{j}"));
                testTest.Add(test);
            }
            List<InlineKeyboardButton> test2 = new List<InlineKeyboardButton>();
            test2.Add(InlineKeyboardButton.WithCallbackData(text: UnifiedStateExam[12], callbackData: $"{12}")); // Добавление кнопки "готово"
            testTest.Add(test2);
            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(testTest);
            return (inlineKeyboard);
        }

        static async void send_message(ITelegramBotClient botClient, ChatId chatId, Update update, CancellationToken cancellationToken, string text) // Функция отправления текстового сообщения
        {
            Message sentmessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: text,
                        cancellationToken: cancellationToken);
        }

        static async void send_inlinekeyboard(ITelegramBotClient botClient, ChatId chatId, Update update, CancellationToken cancellationToken, string text, InlineKeyboardMarkup inlineKeyboard) // Функция отправления inline-клавиатур
        {
            Message sentMessage = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            text: text,
                            replyMarkup: inlineKeyboard,
                            cancellationToken: cancellationToken
                            );
            idmessage_inline = sentMessage.MessageId;
        }

        static async void edit_inlinekeyboard(ITelegramBotClient botClient, ChatId chatId, Update update, CancellationToken cancellationToken, string text, InlineKeyboardMarkup inlineKeyboard) // Редактирование inline-клавиатуры
        {
            Message sentMessage = await botClient.EditMessageTextAsync(
                chatId: chatId,
                messageId: idmessage_inline,
                text: text,
                replyMarkup: inlineKeyboard,
                cancellationToken: cancellationToken);
        }

        /*static async void delete_inlinekeyboard(ITelegramBotClient botClient, ChatId chatId, Update update, CancellationToken cancellationToken, string text, InlineKeyboardMarkup inlineKeyboard) // Редактирование inline-клавиатуры
        {
            Message sentMessage = await botClient.DeleteMessageAsync(
                chatId,
                idmessage_inline,
                cancellationToken)
        }*/

        static async Task Main(string[] args)
        {
            var botClient = new TelegramBotClient("5335914717:AAGMb47HrC4Cec4mhz5HV_cwKmQRtlbEyqA"); // Токен Бота

            using var cts = new CancellationTokenSource();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { } // receive all update types
            };
            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token);

            var me = await botClient.GetMeAsync(); // Запуск самого бота

            Console.WriteLine($"Start listening for @{me.Username}"); // Вывод в консоль запуска бота
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();
        }

        static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            //Console.WriteLine($"Received a '{messageText2}' message in chat {chatId2}.");

            if (update.Type == UpdateType.Message && update.Message.Text != "/start") // Если нам пришло сообщение не с текстом /start
            {
                var chatId2 = update.Message.Chat.Id;
                var message = update.Message.Text; // update.Message.text - записывает в переменную текст который ввел пользователь
                Console.WriteLine($"Пришло сообщение! {message} от {update.Message.Chat.FirstName}");
                int Num;
                bool isNum = int.TryParse(message, out Num);
                if (isNum && message_balls)
                {
                    Console.WriteLine("Ввели сообщение с цифрами");
                    var countballs = Convert.ToInt32(message);
                    summaballov[0] = countballs;
                    Console.WriteLine("Вывод списка факультетов");
                    Console.WriteLine(countballs);
                    { // Выводим список без плюсиков
                        List<List<InlineKeyboardButton>> testTest = new List<List<InlineKeyboardButton>>();
                        for (int i = 20; i < 29; i += 1)
                        {
                            List<InlineKeyboardButton> test = new List<InlineKeyboardButton>();
                            test.Add(InlineKeyboardButton.WithCallbackData(text: Facults[i], callbackData: $"{i}"));
                            testTest.Add(test);
                        }
                        InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(testTest);

                        //send_message(botClient, update, cancellationToken, "Выберите факультет");
                        var chatId = update.Message.Chat.Id;
                        send_inlinekeyboard(botClient, chatId, update, cancellationToken, "Выберите факультет", inlineKeyboard);
                    }
                }
                else
                if (message != "/start") // Если у нас пришло сообщение не с текстом старт то выводим что  не понимаем
                {
                    var chatId = update.Message.Chat.Id;
                    send_message(botClient, chatId, update, cancellationToken, "Я вас не понимаю:(, введите '/start'");
                }
            }
            else if (update.Type == UpdateType.Message && update.Message.Text == "/start") // Аналогично тому что выше, но со стартом
            {
                var chatId = update.Message.Chat.Id;
                zero();
                {
                    send_inlinekeyboard(botClient, chatId, update, cancellationToken, "Выберите предметы", InlineKeyboardUnifiedStateExam());
                }
            }
            else if (update.Type == UpdateType.CallbackQuery) // Обработка всех кнопок
            {
                var chatId = update.CallbackQuery.From.Id;
                Console.WriteLine($"Пользователь {chatId}, {update.CallbackQuery.From.Username} нажал на кнопочку {update.CallbackQuery.Data}");
                var numberknopki = Convert.ToInt32(update.CallbackQuery.Data);
                if (numberknopki == 11)
                {
                    Console.WriteLine("Была нажата отмена");
                    send_message(botClient, chatId, update, cancellationToken, "Надеюсь, что я был полезен!");
                    send_message(botClient, chatId, update, cancellationToken, "Более подробно о специальностях можете прочитать на нашем сайте \nMospolytech.ru/postupayushchim/programmy-obucheniya/");
                    Message message1 = await botClient.SendStickerAsync(
                    chatId: chatId,
                    sticker: "CAACAgIAAxkBAAEEmn5ibpGuN3MxfAumkVpk2_gDwOkHDwACcRgAAvKHAAFIjBZrYgeBmy4kBA",
                    cancellationToken: cancellationToken);
                    zero();
                    return;
                }
                else if (numberknopki == 10) // Если нажали кнопку сброса,обнуляем колво предметов и очищаем плюсики
                {
                    var flag = false;
                    for (int i = 0; i <= 12;i++) // Если клавиатура находится в измененном виде, то только в этом случае можешь ее изменить
                    { // Если два раза нажать сброс, клавиатура будет в одном и том-же состоянии, из-за этого выдает ошибку.
                        if (StandartStateExam[i] != UnifiedStateExam[i])
                            flag = true;
                    }
                    if (flag == true)
                    {
                        zero();
                        edit_inlinekeyboard(botClient, chatId, update, cancellationToken, "Выберите предметы", InlineKeyboardUnifiedStateExam());
                    }
                }
                else // Если выбрали предмет, то счётчик выбранных предметов +=1, и добавляем плюсик возле кнопки
                {
                    select[0] = Convert.ToInt32(numberknopki);
                    if (numberknopki < 10)
                        countlessons[numberknopki] += 1;
                    if ((numberknopki < 10) && (countlessons[numberknopki] < 2)) // Проверка на количество нажатых раз одинаковых предметов чтоб не доджилось.
                    {
                        UnifiedStateExam[numberknopki] += '+'; // Изменяем имя кнопки при ее выборе
                        amount[0] += 1; // Счетчик предметов
                        summapredmetov.Add(numberknopki);
                        edit_inlinekeyboard(botClient, chatId, update, cancellationToken, "Выберите предметы", InlineKeyboardUnifiedStateExam());
                    }


                    if (numberknopki == 12)
                    {

                        send_message(botClient, chatId, update, cancellationToken, "Введите количество баллов в сумме: ");
                        message_balls = true;
                        return;
                    }
                    {
                        if (numberknopki == 20) // Фак графики
                        {
                            if ((summapredmetov.Contains(0)) && (summapredmetov.Contains(9)))
                            {
                                string otvet = "";
                                foreach (var balls in FacultGraphic)
                                {
                                    if (balls.Key <= summaballov[0])
                                    {
                                        otvet += balls.Value + "\n";
                                        otvet += "\n";
                                    }
                                }
                                if (otvet != "")
                                    send_message(botClient, chatId, update, cancellationToken, otvet);
                                else
                                    send_message(botClient, chatId, update, cancellationToken, "С вашим количеством баллов нет шансов поступить на данный факультет\n :(");
                            }
                            else
                                send_message(botClient, chatId, update, cancellationToken, "С текущим набором предметов вы не можете поступить на данный факультет\n :(");
                        }
                        if (numberknopki == 21) // Фак журналистики
                        {
                            if (summapredmetov.Contains(0) && summapredmetov.Contains(9))
                            {
                                string otvet = "";
                                foreach (var balls in FacultPublishingLiterature)
                                {
                                    if (balls.Key <= summaballov[0])
                                    {
                                        otvet += balls.Value + "\n";
                                        otvet += "\n";
                                    }
                                }
                                if (summapredmetov.Contains(9) && summapredmetov.Contains(5))
                                {
                                    foreach (var balls in FacultPublishingOther)
                                    {
                                        if (balls.Key <= summaballov[0])
                                        {
                                            otvet += balls.Value + "\n";
                                            otvet += "\n";
                                        }
                                    }
                                }
                                if (otvet != "")
                                    send_message(botClient, chatId, update, cancellationToken, otvet);
                                else
                                    send_message(botClient, chatId, update, cancellationToken, "С вашим количеством баллов нет шансов поступить на данный факультет\n :(");
                            }
                            else if (summapredmetov.Contains(5) && summapredmetov.Contains(0) && (summapredmetov.Contains(4) || summapredmetov.Contains(9) || summapredmetov.Contains(8)))
                            {
                                string otvet = "";
                                foreach (var balls in FacultPublishingOther)
                                {
                                    if (balls.Key <= summaballov[0])
                                    {
                                        otvet += balls.Value + "\n";
                                        otvet += "\n";
                                    }
                                }
                                if (otvet != "")
                                    send_message(botClient, chatId, update, cancellationToken, otvet);
                                else
                                    send_message(botClient, chatId, update, cancellationToken, "С вашим количеством баллов нет шансов поступить на данный факультет\n :(");
                            }
                            else
                                send_message(botClient, chatId, update, cancellationToken, "С текущим набором предметов вы не можете поступить на данный факультет\n :(");
                        }
                        if ((numberknopki == 22)) // Фит
                        {
                            if ((summapredmetov.Contains(0)) && (summapredmetov.Contains(1)) && (summapredmetov.Contains(6) || summapredmetov.Contains(2)))
                            {
                                string otvet = "";
                                foreach (var balls in FacultIT)
                                {
                                    if (balls.Key <= summaballov[0])
                                    {
                                        otvet += balls.Value + "\n";
                                        otvet += "\n";
                                    }
                                }
                                if (otvet != "")
                                    send_message(botClient, chatId, update, cancellationToken, otvet);
                                else
                                    send_message(botClient, chatId, update, cancellationToken, "С вашим количеством баллов нет шансов поступить на данный факультет\n :(");
                            }
                            else
                                send_message(botClient, chatId, update, cancellationToken, "С текущим набором предметов вы не можете поступить на данный факультет\n :(");
                        }
                        if (numberknopki == 23) // Фак машинстроения
                        {
                            if (summapredmetov.Contains(0) && summapredmetov.Contains(1) && (summapredmetov.Contains(2) || summapredmetov.Contains(6)))
                            {
                                string otvet = "";
                                foreach (var balls in FacultEngineeringIT)
                                {
                                    if (balls.Key <= summaballov[0])
                                    {
                                        otvet += balls.Value + "\n";
                                        otvet += "\n";
                                    }
                                }
                                if (summapredmetov.Contains(2) || summapredmetov.Contains(3))
                                {
                                    foreach (var balls in FacultEngineeringOther)
                                    {
                                        if (balls.Key <= summaballov[0])
                                        {
                                            otvet += balls.Value + "\n";
                                            otvet += "\n";
                                        }
                                    }
                                }
                                if (otvet != "")
                                    send_message(botClient, chatId, update, cancellationToken, otvet);
                                else
                                    send_message(botClient, chatId, update, cancellationToken, "С вашим количеством баллов нет шансов поступить на данный факультет\n :(");
                            }
                            else if (summapredmetov.Contains(0) && summapredmetov.Contains(1) && (summapredmetov.Contains(3) || summapredmetov.Contains(3)))
                            {
                                string otvet = "";
                                foreach (var balls in FacultEngineeringOther)
                                {
                                    if (balls.Key <= summaballov[0])
                                    {
                                        otvet += balls.Value + "\n";
                                        otvet += "\n";
                                    }
                                }
                                if (otvet != "")
                                    send_message(botClient, chatId, update, cancellationToken, otvet);
                                else
                                    send_message(botClient, chatId, update, cancellationToken, "С вашим количеством баллов нет шансов поступить на данный факультет\n :(");
                            }
                            else
                                send_message(botClient, chatId, update, cancellationToken, "С текущим набором предметов вы не можете поступить на данный факультет\n :(");
                        }
                        if ((numberknopki == 24)) // Полиграфический фак
                        {
                            if (summapredmetov.Contains(0) && summapredmetov.Contains(1) && (summapredmetov.Contains(2) || summapredmetov.Contains(6)))
                            {
                                string otvet = "";
                                foreach (var balls in FacultPoligraficInfo)
                                {
                                    if (balls.Key <= summaballov[0])
                                    {
                                        otvet += balls.Value + "\n";
                                        otvet += "\n";
                                    }
                                }
                                if (otvet != "")
                                    send_message(botClient, chatId, update, cancellationToken, otvet);
                                else
                                    send_message(botClient, chatId, update, cancellationToken, "С вашим количеством баллов нет шансов поступить на данный факультет\n :(");
                            }
                            else if (summapredmetov.Contains(0) && summapredmetov.Contains(1) && summapredmetov.Contains(3))
                            {
                                string otvet = "";
                                foreach (var balls in FacultPoligraficOther)
                                {
                                    if (balls.Key <= summaballov[0])
                                    {
                                        otvet += balls.Value + "\n";
                                        otvet += "\n";
                                    }
                                }
                                if (otvet != "")
                                    send_message(botClient, chatId, update, cancellationToken, otvet);
                                else
                                    send_message(botClient, chatId, update, cancellationToken, "С вашим количеством баллов нет шансов поступить на данный факультет\n :(");
                            }
                            else
                                send_message(botClient, chatId, update, cancellationToken, "С текущим набором предметов вы не можете поступить на данный факультет\n :(");
                        }
                        if (numberknopki == 25) // Транспортный фак
                        {
                            if (summapredmetov.Contains(0) && summapredmetov.Contains(1) && (summapredmetov.Contains(2) || summapredmetov.Contains(6)))
                            {
                                string otvet = "";
                                foreach (var balls in FacultTransportInfo)
                                {
                                    if (balls.Key <= summaballov[0])
                                    {
                                        otvet += balls.Value + "\n";
                                        otvet += "\n";
                                    }
                                }
                                if (otvet != "")
                                    send_message(botClient, chatId, update, cancellationToken, otvet);
                                else
                                    send_message(botClient, chatId, update, cancellationToken, "С вашим количеством баллов нет шансов поступить на данный факультет\n :(");
                            }
                            else if (summapredmetov.Contains(0) && (summapredmetov.Contains(4) || summapredmetov.Contains(5) || summapredmetov.Contains(8)))
                            {
                                string otvet = "";
                                foreach (var balls in FacultTransportOther)
                                {
                                    if (balls.Key <= summaballov[0])
                                    {
                                        otvet += balls.Value + "\n";
                                        otvet += "\n";
                                    }
                                }
                                if (otvet != "")
                                    send_message(botClient, chatId, update, cancellationToken, otvet);
                                else
                                    send_message(botClient, chatId, update, cancellationToken, "С вашим количеством баллов нет шансов поступить на данный факультет\n :(");
                            }
                            else
                                send_message(botClient, chatId, update, cancellationToken, "С текущим набором предметов вы не можете поступить на данный факультет\n :(");
                        }
                        if (numberknopki == 26) // Факультет урбанистики
                        {
                            if ((summapredmetov.Contains(0)) && (summapredmetov.Contains(1)) && (summapredmetov.Contains(2) || summapredmetov.Contains(6)))
                            {
                                string otvet = "";
                                foreach (var balls in FacultUrbanInfo)
                                {
                                    if (balls.Key <= summaballov[0])
                                    {
                                        otvet += balls.Value + "\n";
                                        otvet += "\n";
                                    }
                                }
                                if (summapredmetov.Contains(2))
                                {
                                    foreach (var balls in FacultUrbanPhysic)
                                    {
                                        if (balls.Key <= summaballov[0])
                                        {
                                            otvet += balls.Value + "\n";
                                            otvet += "\n";
                                        }
                                    }
                                }
                                if (otvet != "")
                                    send_message(botClient, chatId, update, cancellationToken, otvet);
                                else
                                    send_message(botClient, chatId, update, cancellationToken, "С вашим количеством баллов нет шансов поступить на данный факультет\n :(");
                            }
                            else if ((summapredmetov.Contains(0)) && (summapredmetov.Contains(1)) && (summapredmetov.Contains(2)))
                            {
                                string otvet = "";
                                foreach (var balls in FacultUrbanPhysic)
                                {
                                    if (balls.Key <= summaballov[0])
                                    {
                                        otvet += balls.Value + "\n";
                                        otvet += "\n";
                                    }
                                }
                                if (otvet != "")
                                    send_message(botClient, chatId, update, cancellationToken, otvet);
                                else
                                    send_message(botClient, chatId, update, cancellationToken, "С вашим количеством баллов нет шансов поступить на данный факультет\n :(");
                            }
                            else
                            {
                                send_message(botClient, chatId, update, cancellationToken, "С текущим набором предметов вы не можете поступить на данный факультет\n :(");
                            }
                        }
                        if (numberknopki == 27) // Факультет биотех
                        {
                            if (summapredmetov.Contains(0) && summapredmetov.Contains(1) && (summapredmetov.Contains(2) || summapredmetov.Contains(6)))
                            {
                                string otvet = "";
                                foreach (var balls in FacultBiotechIT)
                                {
                                    if (balls.Key <= summaballov[0])
                                    {
                                        otvet += balls.Value + "\n";
                                        otvet += "\n";
                                    }
                                }
                                if (summapredmetov.Contains(6))
                                {
                                    foreach (var balls in FacultBiotechOther)
                                    {
                                        if (balls.Key <= summaballov[0])
                                        {
                                            otvet += balls.Value + "\n";
                                            otvet += "\n";
                                        }
                                    }
                                }
                                if (otvet != "")
                                    send_message(botClient, chatId, update, cancellationToken, otvet);
                                else
                                    send_message(botClient, chatId, update, cancellationToken, "С вашим количеством баллов нет шансов поступить на данный факультет\n :(");
                            }
                            else if (summapredmetov.Contains(1) && summapredmetov.Contains(0) && (summapredmetov.Contains(7) || summapredmetov.Contains(3)))
                            {
                                string otvet = "";
                                foreach (var balls in FacultBiotechBio)
                                {
                                    if (balls.Key <= summaballov[0])
                                    {
                                        otvet += balls.Value + "\n";
                                        otvet += "\n";
                                    }
                                }
                                if (summapredmetov.Contains(3))
                                {
                                    foreach (var balls in FacultBiotechOther)
                                    {
                                        if (balls.Key <= summaballov[0])
                                        {
                                            otvet += balls.Value + "\n";
                                            otvet += "\n";
                                        }
                                    }
                                }
                                if (otvet != "")
                                    send_message(botClient, chatId, update, cancellationToken, otvet);
                                else
                                    send_message(botClient, chatId, update, cancellationToken, "С вашим количеством баллов нет шансов поступить на данный факультет\n :(");
                            }
                            else
                                send_message(botClient, chatId, update, cancellationToken, "С текущим набором предметов вы не можете поступить на данный факультет\n :(");
                        }
                        if (numberknopki == 28) // Факультет экономики
                        {
                            if (summapredmetov.Contains(0) && summapredmetov.Contains(1) && (summapredmetov.Contains(2) || summapredmetov.Contains(6)))
                            {
                                string otvet = "";
                                foreach (var balls in FacultEconomicIT)
                                {
                                    if (balls.Key <= summaballov[0])
                                    {
                                        otvet += balls.Value + "\n";
                                        otvet += "\n";
                                    }
                                }
                                if (otvet != "")
                                    send_message(botClient, chatId, update, cancellationToken, otvet);
                                else
                                    send_message(botClient, chatId, update, cancellationToken, "С вашим количеством баллов нет шансов поступить на данный факультет\n :(");
                            }
                            else if (summapredmetov.Contains(0) && summapredmetov.Contains(1) && (summapredmetov.Contains(4) || summapredmetov.Contains(5) || summapredmetov.Contains(8)))
                            {
                                string otvet = "";
                                foreach (var balls in FacultEconomicOther)
                                {
                                    if (balls.Key <= summaballov[0])
                                    {
                                        otvet += balls.Value + "\n";
                                        otvet += "\n";
                                    }
                                }
                                if (summapredmetov.Contains(4) || summapredmetov.Contains(8))
                                {
                                    foreach (var balls in FacultEconomicHistory)
                                    {
                                        if (balls.Key <= summaballov[0])
                                        {
                                            otvet += balls.Value + "\n";
                                            otvet += "\n";
                                        }
                                    }
                                }
                                if (otvet != "")
                                    send_message(botClient, chatId, update, cancellationToken, otvet);
                                else
                                    send_message(botClient, chatId, update, cancellationToken, "С вашим количеством баллов нет шансов поступить на данный факультет\n :(");
                            }
                            else
                                send_message(botClient, chatId, update, cancellationToken, "С текущим набором предметов вы не можете поступить на данный факультет\n :(");
                        }
                    }
                    if (numberknopki > 12)
                    {
                        return;
                    }
                    if (amount[0] > 4) // Если выбрано более четырех предметов
                    {
                        send_message(botClient, chatId, update, cancellationToken, "Ошибка! Вы выбрали более четырех предметов");
                        zero();
                        edit_inlinekeyboard(botClient, chatId, update, cancellationToken, "Выберите предметы", InlineKeyboardUnifiedStateExam());
                        return;
                    }
                    //{
                     //   edit_inlinekeyboard(botClient, chatId, update, cancellationToken, "Выберите предметы", InlineKeyboardUnifiedStateExam());
                    //}
                }
            }
            if (update.Type != UpdateType.Message)
                return;
            if (update.Message!.Type != MessageType.Text)
                return;
        }

        static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}