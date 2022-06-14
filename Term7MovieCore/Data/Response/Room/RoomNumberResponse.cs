
using Term7MovieCore.Data.Dto.Room;

namespace Term7MovieCore.Data.Response.Room
{
    public class RoomNumberResponse : ParentResponse
    {
        public IEnumerable<RoomNumberDTO> RoomNumbers { get; set; }
    }
}
/* HASAGI
@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@&&&&&@&&@&@@@@&&&&&@@@@@@@@@@@&&@@&@@####&######&&&&&&&&&&&&&&&&&&@@@@
@@@@@@@@@@@&&&&&&####&&&&&&&&&&&####&&&###&&&####&&##&&&&#&&BB&####Y??55YJ?J??J5GGGGGGGGGGGGGGBB#&@@
@@@@@@@@@@@&&&####BBBBB#&&&&&&&&&##BB##########BBB#####&####GP##BBB!.:!!!~:^....!YYYYYYYYYYYY55GB#&@
@@@@@@@@&&@&&&#B##BBBBBBB##&&&&&&&&&####&#######BB####&&####BP##BBG~.^!~!7:.     ^JYJYYYYYYYYY5PB#&@
@@@@@@@@&&&&@&&##BBBBB#####&&&&&&&&&&&&#&&&############&&###BG##BBG7^~77J?~  ..   ~JYYYYYYYYYY5PB#&@
@@@@@@@@&&&&&&&&&########&&&&&&&&&&&&&&&&&&&&&#########&&###BB##BBBY7JYYY7~.:::.  .?YJJYYYYYYY5PB#&@
@@@@@@@@&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&@@&&&&&&&&&#&&#######BBBYJPP5J7!~!7~.   !JJJYYYYYYY5PB#&@
@@@@@@@&&&&&&&&&&&&&&&&&&&&&####B#####&&&&&&&@@&&&&&&&&&&&######B#BYPGPPY7?YYJ!.   ~JJJJJYYYYY5PB#&@
@@@@@@&&@@&&&&&&&&&&&&&&&&&&###PYJJYYG#GB##&&&&&&&&&&&&&&&&&&&####BPGPBG?:~PP5!.   ~?JYYYYYYYY5PB#&@
@@@@@@&&&&@&&&&&&&&&&&&&&&&&##BY777!7BBJJYG####&&&&&&&&&&&&&&&&###GPGBGJ^..5PY^..  ~?JYYJYYYYY5PB#&@
@@@@@@@&&&@@&&&&&&&&&&&&&&&#G#GJ777~~P#?!J#PJY5G#&#&&&&&&&&&&&@###GB##GJ^..JP?~^.. ~?YYYJJYYYY5PB#&@
@@@@@@@&&&&&&&&&&&&&&&&&&&#GG#GJ777!^7#P^J@Y!JJJJ5BBB#&&&&&@@@@BB##&BY7!~~:~Y?7~~:.!JYYYJJJYYY5PB#&@
@@@@@@@@@&&&&&&&&&&&&&&&&&#BGBGY?77!!^?B?J@Y~7????J55PB#&&&&&@&BBBPJ7~:   .^!!777::7JYYJJJJJYY5PB#&@
@@@@@@@@@@@@&&&&&&&&&&&&&&#GGBGYJ?77!~^!J5&P~!77????JY5PB#&&&&B5G5?!~^:.   :?!~!~.~JJYJJJJYYYY5PB#&@
@@@@@@&##&@@@@&&&&&&&&&&&#BGG#GYY??7!!~^^7#B!!777??JJYYY5GBB#B5Y5J?7~^:.   .7J~^:^?JJJJYYYYYYY5PB#&@
@@@@@&##BGB#&@@&&&@&&&@&#BBGB#5YJ??7!!~~^^?BG??7??JJJYYY55PPGP555Y?!~^.    .7Y!:^!7?YYYYYYYYYY5PB#&@
@@@@@&&&&&BG#&@@&&&&&&&##BBBBP5YJ??77!!!~~^7BB5YJ??JJJY5555PPPP55J?~^:.    .?5~:!!~!77777????JYPB#&@
@@@@@&&&&&&###&@@@&&&&##BBBBGPYJJJ?7!!!!!!!~7GPJY5YJJJYY555PPPP55J?~:..    .?B5PPJ~:.:::::...:~JPB&@
@@@@@&&&&&&&&#&@@@@&&&#BBBGP55YJ7!~~^^~~~!!!~7PGJ?YYYJJY555PPPP5YJ7^:..  .:.7#BBB#B5^.~?!!^^^^!?YG#@
@@@@@&&&#&&&&&&@@@@@&#BBBGGPGB#BPYJ7!~^^^~~!!!!YG5??JYYY55PPPPP5Y?!~^.  .7Y?7GGBBBB#P:!5!!YJ??JYPB#@
@@@@@&&&&&@@@&&&@@@@#BBBBB##&&&&&&&GY7~^~~~~~!!7JPGYJJJY55PPPPP5J??7^. !PY77?PBBBBBB#G5P7:?PGPPGB#&@
@@@@@@&&&&&@&&&@@@@@#GGGGGGGP5Y5B&&&#B5?!!!!!!!7?JYPPYJY55PPPP5YYY?^.^?J?^.:?B###########57^7GBB#&@@
@@@@@@&#&&&@&&&@@@&&BBGGPPPP5YJ77?YG#&&#GY???JJ??JJJ5PP55PPPP5YYJ7:.!J5J!::JB##############P^~P##&&@
@@@@@@@@&&#&&&&@@@&&BBGG555PGGP5J?!!JPB#&##BG55YYYY555Y5PP5P55YJ7:~YB##57!Y########&&#######B7~P#&&@
@@@@@&@@@&&###&@@@&&BBGP5YJY5GGB####BGGGGB#&&&#BBGGGPP5JJ5GG55Y?!Y#&GJ~^7P#########&&&&#######5?P&@@
@@@@&&&&@@&&#B#&@@&#BBGP5YJJ?Y555P5YG&&@@&&&&@&&&&&#BBGPY5GPYY5B&@&5~^.:Y#######&&&&&&&&&######G5G&@
@@@@&&##&@@&&###&&&##BBP5YJJ??JJ!?YJJG#&&&##&@&####&##BPPPP55PB&&#GJJ7:JB#&&&&&&&&&&&&&&&########G#@
@@@@&&####&&&&&#&&&###BGP5YJ??7!~~!YGGB##BBP5GB5PGPPGPPPPBY?##B##GPY!:JB&&&&&&&&&&&&############&&&@
@@@&&#########&&&&&&###BGP5YJJ7!~^~7YPPPPPP5YJJ7!JYJ???YPG~:PGGBBG5J?Y#&&&&&&&&&&&&&#######&&#&&&&&@
@@@&&#BBBBBBBBB##########BP5YYJ?7!!!7JY555PPG5Y5555YJ77J55~.^7Y5PYJJP#&&&&&&&&&&&&&&#######&&&&&&&@@
@@@@&#BGGBBBBBBBBBGPPG####BG5YYJJ??JJJYY55555YJJJYJJ?7?JYJ!~JJJY5YYP#&&&&&&&&&&&&&&&&########&&&&&@@
@@@@@&BGPPPGBBBBBBBBGP5PG###GP5YJJJJJYYYYYYYYY5555YY?7?YYY??555555G#&&&&&&&&&&&&&&&&&#########&&&&@@
@@@@&&##BGP55PGBBGGGBBBGP55GGGP5YYYYY5555Y5555YJJYYYJ?J555??YPPPPG#&&&&&&&&&&&&&&&&&&#########&&&&@@
@@@&&####BBP5YY5PGGGGGGBBBPJJJYYYY5PP555YYYYP555YJ??7?JY5P5?YPGGG#&&&&&&&&&&&&&&&&&&&#########&&&@@@
@@@@&#BBGBBBGP55YJY55PPPPGBBGPY???J555YYJJJJPBBB#BPY?!?Y5PGY5PPG#&&&&&&&&&&&&&&&&&&&&#########&&&@@@
@@@@&#BGGGGBBBGPY???JYYYJJY5PGBBGY777JYYYYYY5B&#&&&#GP5PGBPPGPG#&@&&&&&&&&&&&&&&&&&&##########&&&&@@
@@@@&&#BGPPPGGGBG5J????JJJ????J5PBB5?~^^~!?5G#&&&&&&&#BB#####BB#@@&&&&&&&&&&&&&&&&&############&&&@@
@@@@&&&##GP55PPPPGPY?777?JJJJ????JYPGGGPY?~~?5GB#&@@@&&&&##&&##&@@&&&#&&&&&&&&&&&&#############&&&@@
@@@&&&####GP555P55YPG5Y?7777????????Y5PGBBG5J7!!!7JPB&@@&##&&##&@&&&&#&&&&&&&&&&&##############&&&@@
@@@&&&#BBBBBG5555P5Y5PGP5J?7!!!77?????JYPGGGGBGGP5J?!7?YP#@@&&&@@&&&&&#&&&&&&&&&###############&&&@@
@@@@&&&#BBGGGGP55PP5JJY55PP5Y?7!~!!7???JJY5PGPPPGGBBBBY!^!J5B&@@@@@&####&&&&&&&################&&&@@
@@@@@&&&#BBGPPGGGGGP5Y???JJ55P55J7!~!!!!777JY555PPPGGGPP5YY5J?5##BPYP#&&&&&&###################&&&@@
@@@@@&&&&##BGP5PGGBBBPYJJJ??JJ55PP5Y?7!!!!777?JYY55PPPPGPPB##BBGYJ5B&&&&&&#####################&&&@@
@@@@@@&&&&&&#BGPPPG###BGPP555555PPGBBG5YYYYYYYYYY55PPGGGGGB##&&&#PPG#&&&&##&&&&&&&&&&&&&&&&&&&&&&@@@
@@@@@@@@@@@@@&&##BBB#&&&&####BBBBBBB#######BBBBBBBBBBBBB####&&&&@@@&#####&&&&&&&&&&&@@@@@@@@&@@@@@@@
@@@@@@@@@@@@@@@@@@@@@&@@@@@@@@@@&&&&&&&&@@@@@@@@&&&&&&&&&&@@@@@@@@@@@&&&&@@@@@@@@@@@@@@@@@@@@@@@@@@@



@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@&&&@&&&&&@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@@&&&&&###&&&&&&&&&&&&&@@@@@@@&&#BGGGGGGGGGGGB#&&@@@@#GBGGBBBBBBBBBB#@&##&&&&&@@@@@@@@@@@@@
@@@@@@@@@@@BGGGGGGGGGGGGGGGGGGGGBBBBBBBGGGGGGGGGGGGGGGGGGGB##BGGGGGGGGGGGGGGGGBGGGGGGGGGB@@@@@@@@@@@
@@@@@@@@@@@BGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG&@@@@@@@@@@
@@@@@@@@@@@&GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGB@@@@@@@@@@@
@@@@@@@@@@@&GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG&@@@@@@@@@@
@@@@@@@@@@@&GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG#@@@@@@@@@@
@@@@@@@@@@@#GGGGGGGGBB#&&&&#BGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGBBB##BBGGGGGGGGGGGGGGGGG#@@@@@@@@@@
@@@@@@@@@@@BGGGGGGGG#@@@@@@@@&#BGGGGGGGGGGGGGGGGGGGGGGGGGGB###&&@@@@@@@@&#GGGGGGGGGGGGGGG#@@@@@@@@@@
@@@@@@@@@@&GGGGGGGGGGB&@@@@@@@@@&#GGGGGGGGGGGGGGGGGGGGGGGGB&@@@@@@@@@@@@@@#GGGGGGGGGGGGGG#@@@@@@@@@@
@@@@@@@@@@#GGGGGGGGGGGGB#@@@@@@@@@&#BGGGGGGGGGGGGGGGGGGGGGGGG#&@@@@@@@@@@@BGGGGGGGGGGGGGG#@@@@@@@@@@
@@@@@@@@@@&GGGGGGGGGGGGGGB#@@@@@@@@@@@&#BGGGGGGGGGGGGGGGGGGGGGGB#&@@@@@@@&GGGGGGGGGGGGGGG#@@@@@@@@@@
@@@@@@@@@@@BGGGGGGGGGGGGGGGB#@@@@@@@@@@@@&#BGGGGGGGGGGGGGGGGGGGGGGB#&@@@@#GGGGGGGGGGGGGGG&@@@@@@@@@@
@@@@@@@@@@@&GGGGGGGGGGGGGGGGGG#@@@@@@@@@@@@@&&BGGGGGGGGGGGGGGGGGGGGGGB#&@BGGGGGGGGGGGGGB#@@@@@@@@@@@
@@@@@@@@@@@@BGGGGGGGGGGGGGGGGGGB#@@@@@@@@@@@@@@@&BGGGGGGGGGGGGGGGGGGGGGGBGGGGGGGGGGGGGG&@@@@@@@@@@@@
@@@@@@@@@@@@#GGGGGGGGGGGGGGGGGGGGB#@@@@@@@@@@@@@@@@&#BGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG&@@@@@@@@@@@@@
@@@@@@@@@@@@BGGGGGGGGGGGGGGGGGGGGGGB#@@@@@@@@@@@@@@@@@&#BGGGGGGGGGGGGGGGGGGGGGGGGGGGGGB&@@@@@@@@@@@@
@@@@@@@@@@@@BGGGGGGGGGGGGGGGGGGGGGGGGG#&@@@@@@@@@@@@@@@@@&#BGGGGGGGGGGGGGGGGGGGGGGGGGGGG&@@@@@@@@@@@
@@@@@@@@@@@@@BGGGGGGGGGGGGGGGGGGGGGGGGGG#&@@@@@@@@@@@@@@@@@@&#BGGGGGGGGGGGGGGGGGGGGGGGGG&@@@@@@@@@@@
@@@@@@@@@@@@@#GGGGGGGGGGGGGGGGGGGGGGGGGGGG#@@@@@@@@@@@@@@@@@@@@&#BGGGGGGGGGGGGGGGGGGGGGG&@@@@@@@@@@@
@@@@@@@@@@@#GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG#@@@@@@@@@@@@@@@@@@@@@&&BGGGGGGGGGGGGGGGGGGG&@@@@@@@@@@@
@@@@@@@@@@@BGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG#&@@@@@@@@@@@@@@@@@@@@@@#GGGGGGGGGGGGGGGGB&@@@@@@@@@@@
@@@@@@@@@@@#GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGB&@@@@@@@@@@@@@@@@@@@@@@&&#BGGGGGGGGGGGBG#@@@@@@@@@@
@@@@@@@@@@@#GGGGGGGGGGGGG##GGGGGGGGGGGGGGGGGGGGGGG#&@@@@@@@@@@@@@@@@@@@@@@@@&#BGGGGGGGGGG#@@@@@@@@@@
@@@@@@@@@@@&BGGGGGGGGGGG#@@@#BGGGGGGGGGGGGGGGGGGGGGGB&@@@@@@@@@@@@@@@@@@@@@@@@@&BGGGGGGGG#@@@@@@@@@@
@@@@@@@@@@@@@BGGGGGGGGG#@@@@@@&BGGGGGGGGGGGGGGGGGGGGGGB&@@@@@@@@@@@@@@@@@@@@@@@@BGGGGGGGG#@@@@@@@@@@
@@@@@@@@@@@@@BGGGGGGGGB@@@@@@@@@&BGGGGGGGGGGGGGGGGGGGGGG#&@@@@@@@@@@@@@@@@@@@@@#GGGGGGGGG#@@@@@@@@@@
@@@@@@@@@@@@@BGGGGGGGB@@@@@@@@@@@@&BGGGGGGGGGGGGGGGGGGGGGG#&@@@@@@@@@@@@@@@@@@&GGGGGGGGGG#@@@@@@@@@@
@@@@@@@@@@@@&GGGGGGGG@@@@@@@@@@@@@@@&#BGGGGGGGGGGGGGGGGGGGGG#&@@@@@@@@@@@@@@@@BGGGGGGGGGG#@@@@@@@@@@
@@@@@@@@@@@#GGGGGGGGG&@@@@@@@@@@@@@@@@@&BGGGGGGGGGGGGGGGGGGGGG#&@@@@@@@@@@@@@#GGGGGGGGGGG#@@@@@@@@@@
@@@@@@@@@@@BGGGGGGGGGGB#&@@@@@@@@@@@@&&#BGGGGGGGGGGGGGGGGGGGGGGG#&@@@@@@@@@@#GGGGGGGGGGGG#@@@@@@@@@@
@@@@@@@@@@@BGGGGGGGGGGGGGB#&&&&&&#BBBGGGGGGGGGGGGGGGGGGGGGGGGGGGGGBBBBBB###BGGGGGGGGGGGGG#@@@@@@@@@@
@@@@@@@@@@@#GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG#@@@@@@@@@@
@@@@@@@@@@@&GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG#@@@@@@@@@@
@@@@@@@@@@@&GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG#@@@@@@@@@@
@@@@@@@@@@@BGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGG#@@@@@@@@@@
@@@@@@@@@@@&###BBB########B###BBB####BBBBBBGGGGGGGGGGGGGGGGGGGGGGGGGGGBGGGGGGGGGG#&&&##BG#@@@@@@@@@@
@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@#GB#&#BBBGB#&&&&&&&&&&&&&&@@@&&&&&&&&&@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@&@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
*/