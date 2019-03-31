
import 'dart:collection';
final _p1 = HashSet<int>.from(const [ 256, 258, 260, 262, 264, 266, 268, 270, 272, 274, 276, 278, 280, 282, 284, 286, 288, 290, 292, 294, 296, 298, 300, 302, 306, 308, 310, 313, 315, 317, 319, 321, 323, 325, 327, 330, 332, 334, 336, 338, 340, 342, 344, 346, 348, 350, 352, 354, 356, 358, 360, 362, 364, 366, 368, 370, 372, 374, 377, 379, 381, 386, 388, 391, 395, 401, 408, 416, 418, 420, 423, 428, 431, 435, 437, 440, 444, 453, 456, 459, 461, 463, 465, 467, 469, 471, 473, 475, 478, 480, 482, 484, 486, 488, 490, 492, 494, 498, 500, 504, 506, 508, 510, 512, 514, 516, 518, 520, 522, 524, 526, 528, 530, 532, 534, 536, 538, 540, 542, 546, 548, 550, 552, 554, 556, 558, 560, 562, 571, 577, 582, 584, 586, 588, 590, 880, 882, 886, 984, 986, 988, 990, 992, 994, 996, 998, 1000, 1002, 1004, 1006, 1015, 1018, 1120, 1122, 1124, 1126, 1128, 1130, 1132, 1134, 1136, 1138, 1140, 1142, 1144, 1146, 1148, 1150, 1152, 1162, 1164, 1166, 1168, 1170, 1172, 1174, 1176, 1178, 1180, 1182, 1184, 1186, 1188, 1190, 1192, 1194, 1196, 1198, 1200, 1202, 1204, 1206, 1208, 1210, 1212, 1214, 1217, 1219, 1221, 1223, 1225, 1227, 1229, 1232, 1234, 1236, 1238, 1240, 1242, 1244, 1246, 1248, 1250, 1252, 1254, 1256, 1258, 1260, 1262, 1264, 1266, 1268, 1270, 1272, 1274, 1276, 1278, 1280, 1282, 1284, 1286, 1288, 1290, 1292, 1294, 1296, 1298, 1300, 1302, 1304, 1306, 1308, 1310, 1312, 1314, 7680, 7682, 7684, 7686, 7688, 7690, 7692, 7694, 7696, 7698, 7700, 7702, 7704, 7706, 7708, 7710, 7712, 7714, 7716, 7718, 7720, 7722, 7724, 7726, 7728, 7730, 7732, 7734, 7736, 7738, 7740, 7742, 7744, 7746, 7748, 7750, 7752, 7754, 7756, 7758, 7760, 7762, 7764, 7766, 7768, 7770, 7772, 7774, 7776, 7778, 7780, 7782, 7784, 7786, 7788, 7790, 7792, 7794, 7796, 7798, 7800, 7802, 7804, 7806, 7808, 7810, 7812, 7814, 7816, 7818, 7820, 7822, 7824, 7826, 7828, 7840, 7842, 7844, 7846, 7848, 7850, 7852, 7854, 7856, 7858, 7860, 7862, 7864, 7866, 7868, 7870, 7872, 7874, 7876, 7878, 7880, 7882, 7884, 7886, 7888, 7890, 7892, 7894, 7896, 7898, 7900, 7902, 7904, 7906, 7908, 7910, 7912, 7914, 7916, 7918, 7920, 7922, 7924, 7926, 7928, 7930, 7932, 7934, 8579, 11360, 11367, 11369, 11371, 11378, 11381, 11392, 11394, 11396, 11398, 11400, 11402, 11404, 11406, 11408, 11410, 11412, 11414, 11416, 11418, 11420, 11422, 11424, 11426, 11428, 11430, 11432, 11434, 11436, 11438, 11440, 11442, 11444, 11446, 11448, 11450, 11452, 11454, 11456, 11458, 11460, 11462, 11464, 11466, 11468, 11470, 11472, 11474, 11476, 11478, 11480, 11482, 11484, 11486, 11488, 11490, 42560, 42562, 42564, 42566, 42568, 42570, 42572, 42574, 42576, 42578, 42580, 42582, 42584, 42586, 42588, 42590, 42594, 42596, 42598, 42600, 42602, 42604, 42624, 42626, 42628, 42630, 42632, 42634, 42636, 42638, 42640, 42642, 42644, 42646, 42786, 42788, 42790, 42792, 42794, 42796, 42798, 42802, 42804, 42806, 42808, 42810, 42812, 42814, 42816, 42818, 42820, 42822, 42824, 42826, 42828, 42830, 42832, 42834, 42836, 42838, 42840, 42842, 42844, 42846, 42848, 42850, 42852, 42854, 42856, 42858, 42860, 42862, 42873, 42875, 42878, 42880, 42882, 42884, 42886, 42891 ]);

int netToLowerChar(int ch) {
    if (ch >= 65 && ch <= 90 || ch >= 192 && ch <= 214 || ch >= 216 && ch <= 222 || ch >= 913 && ch <= 929 || ch >= 931 && ch <= 939 || ch >= 1040 && ch <= 1071 || ch >= 65313 && ch <= 65338) return ch + 32;
    if (ch >= 393 && ch <= 394) return ch + 205;
    if (ch >= 433 && ch <= 434) return ch + 217;
    if (ch >= 904 && ch <= 906) return ch + 37;
    if (ch >= 910 && ch <= 911) return ch + 63;
    if (ch >= 1021 && ch <= 1023) return ch + -130;
    if (ch >= 1024 && ch <= 1039) return ch + 80;
    if (ch >= 1329 && ch <= 1366 || ch >= 11264 && ch <= 11310) return ch + 48;
    if (ch >= 4256 && ch <= 4293) return ch + 7264;
    if (ch >= 7944 && ch <= 7951 || ch >= 7960 && ch <= 7965 || ch >= 7976 && ch <= 7983 || ch >= 7992 && ch <= 7999 || ch >= 8008 && ch <= 8013 || ch >= 8040 && ch <= 8047 || ch >= 8072 && ch <= 8079 || ch >= 8088 && ch <= 8095 || ch >= 8104 && ch <= 8111 || ch >= 8120 && ch <= 8121 || ch >= 8152 && ch <= 8153 || ch >= 8168 && ch <= 8169) return ch + -8;
    if (ch >= 8122 && ch <= 8123) return ch + -74;
    if (ch >= 8136 && ch <= 8139) return ch + -86;
    if (ch >= 8154 && ch <= 8155) return ch + -100;
    if (ch >= 8170 && ch <= 8171) return ch + -112;
    if (ch >= 8184 && ch <= 8185) return ch + -128;
    if (ch >= 8186 && ch <= 8187) return ch + -126;
    if (ch >= 8544 && ch <= 8559) return ch + 16;
    if (ch >= 9398 && ch <= 9423) return ch + 26;
    if (_p1.contains(ch)) return ch + 1;
    switch (ch) {
      case 304: return 105;
      case 376: return 255;
      case 385: return 595;
      case 390: return 596;
      case 398: return 477;
      case 399: return 601;
      case 400: return 603;
      case 403: return 608;
      case 404: return 611;
      case 406: return 617;
      case 407: return 616;
      case 412: return 623;
      case 413: return 626;
      case 415: return 629;
      case 422: return 640;
      case 425: return 643;
      case 430: return 648;
      case 439: return 658;
      case 452: return 454;
      case 455: return 457;
      case 458: return 460;
      case 497: return 499;
      case 502: return 405;
      case 503: return 447;
      case 544: return 414;
      case 570: return 11365;
      case 573: return 410;
      case 574: return 11366;
      case 579: return 384;
      case 580: return 649;
      case 581: return 652;
      case 902: return 940;
      case 908: return 972;
      case 975: return 983;
      case 978: return 965;
      case 979: return 973;
      case 980: return 971;
      case 1012: return 952;
      case 1017: return 1010;
      case 1216: return 1231;
      case 7838: return 223;
      case 8025: return 8017;
      case 8027: return 8019;
      case 8029: return 8021;
      case 8031: return 8023;
      case 8124: return 8115;
      case 8140: return 8131;
      case 8172: return 8165;
      case 8188: return 8179;
      case 8486: return 969;
      case 8490: return 107;
      case 8491: return 229;
      case 8498: return 8526;
      case 11362: return 619;
      case 11363: return 7549;
      case 11364: return 637;
      case 11373: return 593;
      case 11374: return 625;
      case 11375: return 592;
      case 42877: return 7545;
      default: return ch;
    }
}
