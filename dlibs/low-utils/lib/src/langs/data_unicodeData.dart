
import 'package:rewise_low_utils/messages.dart' show UncBlocks;

UncBlocks getUnicodeData() {
  if (_unicodeData==null) {
    const res = '''
{
  "ISO15924": [ "Latn", "Zyyy", "Grek", "Copt", "Cyrl", "Armn", "Hebr", "Arab", "Syrc", "Thaa", "Nkoo", "Samr", "Mand", "Deva", "Beng", "Guru", "Gujr", "Orya", "Taml", "Telu", "Knda", "Mlym", "Sinh", "Thai", "Laoo", "Tibt", "Mymr", "Geor", "Hang", "Ethi", "Cher", "Cans", "Ogam", "Runr", "Tglg", "Hano", "Buhd", "Tagb", "Khmr", "Mong", "Limb", "Tale", "Talu", "Bugi", "Lana", "Bali", "Sund", "Batk", "Lepc", "Olck", "Glag", "Tfng", "Hira", "Kana", "Bopo", "Hani", "Yiii", "Lisu", "Vaii", "Bamu", "Sylo", "Phag", "Saur", "Kali", "Rjng", "Java", "Cham", "Tavt", "Mtei" ],
  "ranges": [
    {
      "start": 65,
      "end": 90
    },
    {
      "start": 97,
      "end": 122
    },
    {
      "start": 170,
      "end": 170
    },
    {
      "start": 181,
      "end": 181,
      "idx": 1
    },
    {
      "start": 186,
      "end": 186
    },
    {
      "start": 192,
      "end": 214
    },
    {
      "start": 216,
      "end": 246
    },
    {
      "start": 248,
      "end": 442
    },
    {
      "start": 443,
      "end": 443
    },
    {
      "start": 444,
      "end": 447
    },
    {
      "start": 448,
      "end": 451
    },
    {
      "start": 452,
      "end": 659
    },
    {
      "start": 660,
      "end": 660
    },
    {
      "start": 661,
      "end": 687
    },
    {
      "start": 880,
      "end": 883,
      "idx": 2
    },
    {
      "start": 886,
      "end": 887,
      "idx": 2
    },
    {
      "start": 891,
      "end": 893,
      "idx": 2
    },
    {
      "start": 895,
      "end": 895,
      "idx": 2
    },
    {
      "start": 902,
      "end": 902,
      "idx": 2
    },
    {
      "start": 904,
      "end": 906,
      "idx": 2
    },
    {
      "start": 908,
      "end": 908,
      "idx": 2
    },
    {
      "start": 910,
      "end": 929,
      "idx": 2
    },
    {
      "start": 931,
      "end": 993,
      "idx": 2
    },
    {
      "start": 994,
      "end": 1007,
      "idx": 3
    },
    {
      "start": 1008,
      "end": 1013,
      "idx": 2
    },
    {
      "start": 1015,
      "end": 1023,
      "idx": 2
    },
    {
      "start": 1024,
      "end": 1153,
      "idx": 4
    },
    {
      "start": 1162,
      "end": 1327,
      "idx": 4
    },
    {
      "start": 1329,
      "end": 1366,
      "idx": 5
    },
    {
      "start": 1376,
      "end": 1416,
      "idx": 5
    },
    {
      "start": 1488,
      "end": 1514,
      "idx": 6
    },
    {
      "start": 1519,
      "end": 1522,
      "idx": 6
    },
    {
      "start": 1568,
      "end": 1599,
      "idx": 7
    },
    {
      "start": 1601,
      "end": 1610,
      "idx": 7
    },
    {
      "start": 1646,
      "end": 1647,
      "idx": 7
    },
    {
      "start": 1649,
      "end": 1747,
      "idx": 7
    },
    {
      "start": 1749,
      "end": 1749,
      "idx": 7
    },
    {
      "start": 1774,
      "end": 1775,
      "idx": 7
    },
    {
      "start": 1786,
      "end": 1788,
      "idx": 7
    },
    {
      "start": 1791,
      "end": 1791,
      "idx": 7
    },
    {
      "start": 1808,
      "end": 1808,
      "idx": 8
    },
    {
      "start": 1810,
      "end": 1839,
      "idx": 8
    },
    {
      "start": 1869,
      "end": 1871,
      "idx": 8
    },
    {
      "start": 1872,
      "end": 1919,
      "idx": 7
    },
    {
      "start": 1920,
      "end": 1957,
      "idx": 9
    },
    {
      "start": 1969,
      "end": 1969,
      "idx": 9
    },
    {
      "start": 1994,
      "end": 2026,
      "idx": 10
    },
    {
      "start": 2048,
      "end": 2069,
      "idx": 11
    },
    {
      "start": 2112,
      "end": 2136,
      "idx": 12
    },
    {
      "start": 2144,
      "end": 2154,
      "idx": 8
    },
    {
      "start": 2208,
      "end": 2228,
      "idx": 7
    },
    {
      "start": 2230,
      "end": 2237,
      "idx": 7
    },
    {
      "start": 2308,
      "end": 2361,
      "idx": 13
    },
    {
      "start": 2365,
      "end": 2365,
      "idx": 13
    },
    {
      "start": 2384,
      "end": 2384,
      "idx": 13
    },
    {
      "start": 2392,
      "end": 2401,
      "idx": 13
    },
    {
      "start": 2418,
      "end": 2431,
      "idx": 13
    },
    {
      "start": 2432,
      "end": 2432,
      "idx": 14
    },
    {
      "start": 2437,
      "end": 2444,
      "idx": 14
    },
    {
      "start": 2447,
      "end": 2448,
      "idx": 14
    },
    {
      "start": 2451,
      "end": 2472,
      "idx": 14
    },
    {
      "start": 2474,
      "end": 2480,
      "idx": 14
    },
    {
      "start": 2482,
      "end": 2482,
      "idx": 14
    },
    {
      "start": 2486,
      "end": 2489,
      "idx": 14
    },
    {
      "start": 2493,
      "end": 2493,
      "idx": 14
    },
    {
      "start": 2510,
      "end": 2510,
      "idx": 14
    },
    {
      "start": 2524,
      "end": 2525,
      "idx": 14
    },
    {
      "start": 2527,
      "end": 2529,
      "idx": 14
    },
    {
      "start": 2544,
      "end": 2545,
      "idx": 14
    },
    {
      "start": 2556,
      "end": 2556,
      "idx": 14
    },
    {
      "start": 2565,
      "end": 2570,
      "idx": 15
    },
    {
      "start": 2575,
      "end": 2576,
      "idx": 15
    },
    {
      "start": 2579,
      "end": 2600,
      "idx": 15
    },
    {
      "start": 2602,
      "end": 2608,
      "idx": 15
    },
    {
      "start": 2610,
      "end": 2611,
      "idx": 15
    },
    {
      "start": 2613,
      "end": 2614,
      "idx": 15
    },
    {
      "start": 2616,
      "end": 2617,
      "idx": 15
    },
    {
      "start": 2649,
      "end": 2652,
      "idx": 15
    },
    {
      "start": 2654,
      "end": 2654,
      "idx": 15
    },
    {
      "start": 2674,
      "end": 2676,
      "idx": 15
    },
    {
      "start": 2693,
      "end": 2701,
      "idx": 16
    },
    {
      "start": 2703,
      "end": 2705,
      "idx": 16
    },
    {
      "start": 2707,
      "end": 2728,
      "idx": 16
    },
    {
      "start": 2730,
      "end": 2736,
      "idx": 16
    },
    {
      "start": 2738,
      "end": 2739,
      "idx": 16
    },
    {
      "start": 2741,
      "end": 2745,
      "idx": 16
    },
    {
      "start": 2749,
      "end": 2749,
      "idx": 16
    },
    {
      "start": 2768,
      "end": 2768,
      "idx": 16
    },
    {
      "start": 2784,
      "end": 2785,
      "idx": 16
    },
    {
      "start": 2809,
      "end": 2809,
      "idx": 16
    },
    {
      "start": 2821,
      "end": 2828,
      "idx": 17
    },
    {
      "start": 2831,
      "end": 2832,
      "idx": 17
    },
    {
      "start": 2835,
      "end": 2856,
      "idx": 17
    },
    {
      "start": 2858,
      "end": 2864,
      "idx": 17
    },
    {
      "start": 2866,
      "end": 2867,
      "idx": 17
    },
    {
      "start": 2869,
      "end": 2873,
      "idx": 17
    },
    {
      "start": 2877,
      "end": 2877,
      "idx": 17
    },
    {
      "start": 2908,
      "end": 2909,
      "idx": 17
    },
    {
      "start": 2911,
      "end": 2913,
      "idx": 17
    },
    {
      "start": 2929,
      "end": 2929,
      "idx": 17
    },
    {
      "start": 2947,
      "end": 2947,
      "idx": 18
    },
    {
      "start": 2949,
      "end": 2954,
      "idx": 18
    },
    {
      "start": 2958,
      "end": 2960,
      "idx": 18
    },
    {
      "start": 2962,
      "end": 2965,
      "idx": 18
    },
    {
      "start": 2969,
      "end": 2970,
      "idx": 18
    },
    {
      "start": 2972,
      "end": 2972,
      "idx": 18
    },
    {
      "start": 2974,
      "end": 2975,
      "idx": 18
    },
    {
      "start": 2979,
      "end": 2980,
      "idx": 18
    },
    {
      "start": 2984,
      "end": 2986,
      "idx": 18
    },
    {
      "start": 2990,
      "end": 3001,
      "idx": 18
    },
    {
      "start": 3024,
      "end": 3024,
      "idx": 18
    },
    {
      "start": 3077,
      "end": 3084,
      "idx": 19
    },
    {
      "start": 3086,
      "end": 3088,
      "idx": 19
    },
    {
      "start": 3090,
      "end": 3112,
      "idx": 19
    },
    {
      "start": 3114,
      "end": 3129,
      "idx": 19
    },
    {
      "start": 3133,
      "end": 3133,
      "idx": 19
    },
    {
      "start": 3160,
      "end": 3162,
      "idx": 19
    },
    {
      "start": 3168,
      "end": 3169,
      "idx": 19
    },
    {
      "start": 3200,
      "end": 3200,
      "idx": 20
    },
    {
      "start": 3205,
      "end": 3212,
      "idx": 20
    },
    {
      "start": 3214,
      "end": 3216,
      "idx": 20
    },
    {
      "start": 3218,
      "end": 3240,
      "idx": 20
    },
    {
      "start": 3242,
      "end": 3251,
      "idx": 20
    },
    {
      "start": 3253,
      "end": 3257,
      "idx": 20
    },
    {
      "start": 3261,
      "end": 3261,
      "idx": 20
    },
    {
      "start": 3294,
      "end": 3294,
      "idx": 20
    },
    {
      "start": 3296,
      "end": 3297,
      "idx": 20
    },
    {
      "start": 3313,
      "end": 3314,
      "idx": 20
    },
    {
      "start": 3333,
      "end": 3340,
      "idx": 21
    },
    {
      "start": 3342,
      "end": 3344,
      "idx": 21
    },
    {
      "start": 3346,
      "end": 3386,
      "idx": 21
    },
    {
      "start": 3389,
      "end": 3389,
      "idx": 21
    },
    {
      "start": 3406,
      "end": 3406,
      "idx": 21
    },
    {
      "start": 3412,
      "end": 3414,
      "idx": 21
    },
    {
      "start": 3423,
      "end": 3425,
      "idx": 21
    },
    {
      "start": 3450,
      "end": 3455,
      "idx": 21
    },
    {
      "start": 3461,
      "end": 3478,
      "idx": 22
    },
    {
      "start": 3482,
      "end": 3505,
      "idx": 22
    },
    {
      "start": 3507,
      "end": 3515,
      "idx": 22
    },
    {
      "start": 3517,
      "end": 3517,
      "idx": 22
    },
    {
      "start": 3520,
      "end": 3526,
      "idx": 22
    },
    {
      "start": 3585,
      "end": 3632,
      "idx": 23
    },
    {
      "start": 3634,
      "end": 3635,
      "idx": 23
    },
    {
      "start": 3648,
      "end": 3653,
      "idx": 23
    },
    {
      "start": 3713,
      "end": 3714,
      "idx": 24
    },
    {
      "start": 3716,
      "end": 3716,
      "idx": 24
    },
    {
      "start": 3719,
      "end": 3720,
      "idx": 24
    },
    {
      "start": 3722,
      "end": 3722,
      "idx": 24
    },
    {
      "start": 3725,
      "end": 3725,
      "idx": 24
    },
    {
      "start": 3732,
      "end": 3735,
      "idx": 24
    },
    {
      "start": 3737,
      "end": 3743,
      "idx": 24
    },
    {
      "start": 3745,
      "end": 3747,
      "idx": 24
    },
    {
      "start": 3749,
      "end": 3749,
      "idx": 24
    },
    {
      "start": 3751,
      "end": 3751,
      "idx": 24
    },
    {
      "start": 3754,
      "end": 3755,
      "idx": 24
    },
    {
      "start": 3757,
      "end": 3760,
      "idx": 24
    },
    {
      "start": 3762,
      "end": 3763,
      "idx": 24
    },
    {
      "start": 3773,
      "end": 3773,
      "idx": 24
    },
    {
      "start": 3776,
      "end": 3780,
      "idx": 24
    },
    {
      "start": 3804,
      "end": 3807,
      "idx": 24
    },
    {
      "start": 3840,
      "end": 3840,
      "idx": 25
    },
    {
      "start": 3904,
      "end": 3911,
      "idx": 25
    },
    {
      "start": 3913,
      "end": 3948,
      "idx": 25
    },
    {
      "start": 3976,
      "end": 3980,
      "idx": 25
    },
    {
      "start": 4096,
      "end": 4138,
      "idx": 26
    },
    {
      "start": 4159,
      "end": 4159,
      "idx": 26
    },
    {
      "start": 4176,
      "end": 4181,
      "idx": 26
    },
    {
      "start": 4186,
      "end": 4189,
      "idx": 26
    },
    {
      "start": 4193,
      "end": 4193,
      "idx": 26
    },
    {
      "start": 4197,
      "end": 4198,
      "idx": 26
    },
    {
      "start": 4206,
      "end": 4208,
      "idx": 26
    },
    {
      "start": 4213,
      "end": 4225,
      "idx": 26
    },
    {
      "start": 4238,
      "end": 4238,
      "idx": 26
    },
    {
      "start": 4256,
      "end": 4293,
      "idx": 27
    },
    {
      "start": 4295,
      "end": 4295,
      "idx": 27
    },
    {
      "start": 4301,
      "end": 4301,
      "idx": 27
    },
    {
      "start": 4304,
      "end": 4346,
      "idx": 27
    },
    {
      "start": 4349,
      "end": 4351,
      "idx": 27
    },
    {
      "start": 4352,
      "end": 4607,
      "idx": 28
    },
    {
      "start": 4608,
      "end": 4680,
      "idx": 29
    },
    {
      "start": 4682,
      "end": 4685,
      "idx": 29
    },
    {
      "start": 4688,
      "end": 4694,
      "idx": 29
    },
    {
      "start": 4696,
      "end": 4696,
      "idx": 29
    },
    {
      "start": 4698,
      "end": 4701,
      "idx": 29
    },
    {
      "start": 4704,
      "end": 4744,
      "idx": 29
    },
    {
      "start": 4746,
      "end": 4749,
      "idx": 29
    },
    {
      "start": 4752,
      "end": 4784,
      "idx": 29
    },
    {
      "start": 4786,
      "end": 4789,
      "idx": 29
    },
    {
      "start": 4792,
      "end": 4798,
      "idx": 29
    },
    {
      "start": 4800,
      "end": 4800,
      "idx": 29
    },
    {
      "start": 4802,
      "end": 4805,
      "idx": 29
    },
    {
      "start": 4808,
      "end": 4822,
      "idx": 29
    },
    {
      "start": 4824,
      "end": 4880,
      "idx": 29
    },
    {
      "start": 4882,
      "end": 4885,
      "idx": 29
    },
    {
      "start": 4888,
      "end": 4954,
      "idx": 29
    },
    {
      "start": 4992,
      "end": 5007,
      "idx": 29
    },
    {
      "start": 5024,
      "end": 5109,
      "idx": 30
    },
    {
      "start": 5112,
      "end": 5117,
      "idx": 30
    },
    {
      "start": 5121,
      "end": 5740,
      "idx": 31
    },
    {
      "start": 5743,
      "end": 5759,
      "idx": 31
    },
    {
      "start": 5761,
      "end": 5786,
      "idx": 32
    },
    {
      "start": 5792,
      "end": 5866,
      "idx": 33
    },
    {
      "start": 5873,
      "end": 5880,
      "idx": 33
    },
    {
      "start": 5888,
      "end": 5900,
      "idx": 34
    },
    {
      "start": 5902,
      "end": 5905,
      "idx": 34
    },
    {
      "start": 5920,
      "end": 5937,
      "idx": 35
    },
    {
      "start": 5952,
      "end": 5969,
      "idx": 36
    },
    {
      "start": 5984,
      "end": 5996,
      "idx": 37
    },
    {
      "start": 5998,
      "end": 6000,
      "idx": 37
    },
    {
      "start": 6016,
      "end": 6067,
      "idx": 38
    },
    {
      "start": 6108,
      "end": 6108,
      "idx": 38
    },
    {
      "start": 6176,
      "end": 6210,
      "idx": 39
    },
    {
      "start": 6212,
      "end": 6264,
      "idx": 39
    },
    {
      "start": 6272,
      "end": 6276,
      "idx": 39
    },
    {
      "start": 6279,
      "end": 6312,
      "idx": 39
    },
    {
      "start": 6314,
      "end": 6314,
      "idx": 39
    },
    {
      "start": 6320,
      "end": 6389,
      "idx": 31
    },
    {
      "start": 6400,
      "end": 6430,
      "idx": 40
    },
    {
      "start": 6480,
      "end": 6509,
      "idx": 41
    },
    {
      "start": 6512,
      "end": 6516,
      "idx": 41
    },
    {
      "start": 6528,
      "end": 6571,
      "idx": 42
    },
    {
      "start": 6576,
      "end": 6601,
      "idx": 42
    },
    {
      "start": 6656,
      "end": 6678,
      "idx": 43
    },
    {
      "start": 6688,
      "end": 6740,
      "idx": 44
    },
    {
      "start": 6917,
      "end": 6963,
      "idx": 45
    },
    {
      "start": 6981,
      "end": 6987,
      "idx": 45
    },
    {
      "start": 7043,
      "end": 7072,
      "idx": 46
    },
    {
      "start": 7086,
      "end": 7087,
      "idx": 46
    },
    {
      "start": 7098,
      "end": 7103,
      "idx": 46
    },
    {
      "start": 7104,
      "end": 7141,
      "idx": 47
    },
    {
      "start": 7168,
      "end": 7203,
      "idx": 48
    },
    {
      "start": 7245,
      "end": 7247,
      "idx": 48
    },
    {
      "start": 7258,
      "end": 7287,
      "idx": 49
    },
    {
      "start": 7296,
      "end": 7304,
      "idx": 4
    },
    {
      "start": 7312,
      "end": 7354,
      "idx": 27
    },
    {
      "start": 7357,
      "end": 7359,
      "idx": 27
    },
    {
      "start": 7401,
      "end": 7404,
      "idx": 1
    },
    {
      "start": 7406,
      "end": 7409,
      "idx": 1
    },
    {
      "start": 7413,
      "end": 7414,
      "idx": 1
    },
    {
      "start": 7424,
      "end": 7461
    },
    {
      "start": 7462,
      "end": 7466,
      "idx": 2
    },
    {
      "start": 7467,
      "end": 7467,
      "idx": 4
    },
    {
      "start": 7531,
      "end": 7543
    },
    {
      "start": 7545,
      "end": 7578
    },
    {
      "start": 7680,
      "end": 7935
    },
    {
      "start": 7936,
      "end": 7957,
      "idx": 2
    },
    {
      "start": 7960,
      "end": 7965,
      "idx": 2
    },
    {
      "start": 7968,
      "end": 8005,
      "idx": 2
    },
    {
      "start": 8008,
      "end": 8013,
      "idx": 2
    },
    {
      "start": 8016,
      "end": 8023,
      "idx": 2
    },
    {
      "start": 8025,
      "end": 8025,
      "idx": 2
    },
    {
      "start": 8027,
      "end": 8027,
      "idx": 2
    },
    {
      "start": 8029,
      "end": 8029,
      "idx": 2
    },
    {
      "start": 8031,
      "end": 8061,
      "idx": 2
    },
    {
      "start": 8064,
      "end": 8116,
      "idx": 2
    },
    {
      "start": 8118,
      "end": 8124,
      "idx": 2
    },
    {
      "start": 8126,
      "end": 8126,
      "idx": 2
    },
    {
      "start": 8130,
      "end": 8132,
      "idx": 2
    },
    {
      "start": 8134,
      "end": 8140,
      "idx": 2
    },
    {
      "start": 8144,
      "end": 8147,
      "idx": 2
    },
    {
      "start": 8150,
      "end": 8155,
      "idx": 2
    },
    {
      "start": 8160,
      "end": 8172,
      "idx": 2
    },
    {
      "start": 8178,
      "end": 8180,
      "idx": 2
    },
    {
      "start": 8182,
      "end": 8188,
      "idx": 2
    },
    {
      "start": 8450,
      "end": 8450,
      "idx": 1
    },
    {
      "start": 8455,
      "end": 8455,
      "idx": 1
    },
    {
      "start": 8458,
      "end": 8467,
      "idx": 1
    },
    {
      "start": 8469,
      "end": 8469,
      "idx": 1
    },
    {
      "start": 8473,
      "end": 8477,
      "idx": 1
    },
    {
      "start": 8484,
      "end": 8484,
      "idx": 1
    },
    {
      "start": 8486,
      "end": 8486,
      "idx": 2
    },
    {
      "start": 8488,
      "end": 8488,
      "idx": 1
    },
    {
      "start": 8490,
      "end": 8491
    },
    {
      "start": 8492,
      "end": 8493,
      "idx": 1
    },
    {
      "start": 8495,
      "end": 8497,
      "idx": 1
    },
    {
      "start": 8498,
      "end": 8498
    },
    {
      "start": 8499,
      "end": 8500,
      "idx": 1
    },
    {
      "start": 8501,
      "end": 8504,
      "idx": 1
    },
    {
      "start": 8505,
      "end": 8505,
      "idx": 1
    },
    {
      "start": 8508,
      "end": 8511,
      "idx": 1
    },
    {
      "start": 8517,
      "end": 8521,
      "idx": 1
    },
    {
      "start": 8526,
      "end": 8526
    },
    {
      "start": 8579,
      "end": 8580
    },
    {
      "start": 11264,
      "end": 11310,
      "idx": 50
    },
    {
      "start": 11312,
      "end": 11358,
      "idx": 50
    },
    {
      "start": 11360,
      "end": 11387
    },
    {
      "start": 11390,
      "end": 11391
    },
    {
      "start": 11392,
      "end": 11492,
      "idx": 3
    },
    {
      "start": 11499,
      "end": 11502,
      "idx": 3
    },
    {
      "start": 11506,
      "end": 11507,
      "idx": 3
    },
    {
      "start": 11520,
      "end": 11557,
      "idx": 27
    },
    {
      "start": 11559,
      "end": 11559,
      "idx": 27
    },
    {
      "start": 11565,
      "end": 11565,
      "idx": 27
    },
    {
      "start": 11568,
      "end": 11623,
      "idx": 51
    },
    {
      "start": 11648,
      "end": 11670,
      "idx": 29
    },
    {
      "start": 11680,
      "end": 11686,
      "idx": 29
    },
    {
      "start": 11688,
      "end": 11694,
      "idx": 29
    },
    {
      "start": 11696,
      "end": 11702,
      "idx": 29
    },
    {
      "start": 11704,
      "end": 11710,
      "idx": 29
    },
    {
      "start": 11712,
      "end": 11718,
      "idx": 29
    },
    {
      "start": 11720,
      "end": 11726,
      "idx": 29
    },
    {
      "start": 11728,
      "end": 11734,
      "idx": 29
    },
    {
      "start": 11736,
      "end": 11742,
      "idx": 29
    },
    {
      "start": 12294,
      "end": 12294,
      "idx": 1
    },
    {
      "start": 12348,
      "end": 12348,
      "idx": 1
    },
    {
      "start": 12353,
      "end": 12438,
      "idx": 52
    },
    {
      "start": 12447,
      "end": 12447,
      "idx": 52
    },
    {
      "start": 12449,
      "end": 12538,
      "idx": 53
    },
    {
      "start": 12543,
      "end": 12543,
      "idx": 53
    },
    {
      "start": 12549,
      "end": 12591,
      "idx": 54
    },
    {
      "start": 12593,
      "end": 12686,
      "idx": 28
    },
    {
      "start": 12704,
      "end": 12730,
      "idx": 54
    },
    {
      "start": 12784,
      "end": 12799,
      "idx": 53
    },
    {
      "start": 13312,
      "end": 19893,
      "idx": 55
    },
    {
      "start": 19968,
      "end": 40943,
      "idx": 55
    },
    {
      "start": 40960,
      "end": 40980,
      "idx": 56
    },
    {
      "start": 40982,
      "end": 42124,
      "idx": 56
    },
    {
      "start": 42192,
      "end": 42231,
      "idx": 57
    },
    {
      "start": 42240,
      "end": 42507,
      "idx": 58
    },
    {
      "start": 42512,
      "end": 42527,
      "idx": 58
    },
    {
      "start": 42538,
      "end": 42539,
      "idx": 58
    },
    {
      "start": 42560,
      "end": 42605,
      "idx": 4
    },
    {
      "start": 42606,
      "end": 42606,
      "idx": 4
    },
    {
      "start": 42624,
      "end": 42651,
      "idx": 4
    },
    {
      "start": 42656,
      "end": 42725,
      "idx": 59
    },
    {
      "start": 42786,
      "end": 42863
    },
    {
      "start": 42865,
      "end": 42887
    },
    {
      "start": 42891,
      "end": 42894
    },
    {
      "start": 42895,
      "end": 42895
    },
    {
      "start": 42896,
      "end": 42937
    },
    {
      "start": 42999,
      "end": 42999
    },
    {
      "start": 43002,
      "end": 43002
    },
    {
      "start": 43003,
      "end": 43007
    },
    {
      "start": 43008,
      "end": 43009,
      "idx": 60
    },
    {
      "start": 43011,
      "end": 43013,
      "idx": 60
    },
    {
      "start": 43015,
      "end": 43018,
      "idx": 60
    },
    {
      "start": 43020,
      "end": 43042,
      "idx": 60
    },
    {
      "start": 43072,
      "end": 43123,
      "idx": 61
    },
    {
      "start": 43138,
      "end": 43187,
      "idx": 62
    },
    {
      "start": 43250,
      "end": 43255,
      "idx": 13
    },
    {
      "start": 43259,
      "end": 43259,
      "idx": 13
    },
    {
      "start": 43261,
      "end": 43262,
      "idx": 13
    },
    {
      "start": 43274,
      "end": 43301,
      "idx": 63
    },
    {
      "start": 43312,
      "end": 43334,
      "idx": 64
    },
    {
      "start": 43360,
      "end": 43388,
      "idx": 28
    },
    {
      "start": 43396,
      "end": 43442,
      "idx": 65
    },
    {
      "start": 43488,
      "end": 43492,
      "idx": 26
    },
    {
      "start": 43495,
      "end": 43503,
      "idx": 26
    },
    {
      "start": 43514,
      "end": 43518,
      "idx": 26
    },
    {
      "start": 43520,
      "end": 43560,
      "idx": 66
    },
    {
      "start": 43584,
      "end": 43586,
      "idx": 66
    },
    {
      "start": 43588,
      "end": 43595,
      "idx": 66
    },
    {
      "start": 43616,
      "end": 43631,
      "idx": 26
    },
    {
      "start": 43633,
      "end": 43638,
      "idx": 26
    },
    {
      "start": 43642,
      "end": 43642,
      "idx": 26
    },
    {
      "start": 43646,
      "end": 43647,
      "idx": 26
    },
    {
      "start": 43648,
      "end": 43695,
      "idx": 67
    },
    {
      "start": 43697,
      "end": 43697,
      "idx": 67
    },
    {
      "start": 43701,
      "end": 43702,
      "idx": 67
    },
    {
      "start": 43705,
      "end": 43709,
      "idx": 67
    },
    {
      "start": 43712,
      "end": 43712,
      "idx": 67
    },
    {
      "start": 43714,
      "end": 43714,
      "idx": 67
    },
    {
      "start": 43739,
      "end": 43740,
      "idx": 67
    },
    {
      "start": 43744,
      "end": 43754,
      "idx": 68
    },
    {
      "start": 43762,
      "end": 43762,
      "idx": 68
    },
    {
      "start": 43777,
      "end": 43782,
      "idx": 29
    },
    {
      "start": 43785,
      "end": 43790,
      "idx": 29
    },
    {
      "start": 43793,
      "end": 43798,
      "idx": 29
    },
    {
      "start": 43808,
      "end": 43814,
      "idx": 29
    },
    {
      "start": 43816,
      "end": 43822,
      "idx": 29
    },
    {
      "start": 43824,
      "end": 43866
    },
    {
      "start": 43872,
      "end": 43876
    },
    {
      "start": 43877,
      "end": 43877,
      "idx": 2
    },
    {
      "start": 43888,
      "end": 43967,
      "idx": 30
    },
    {
      "start": 43968,
      "end": 44002,
      "idx": 68
    },
    {
      "start": 44032,
      "end": 55203,
      "idx": 28
    },
    {
      "start": 55216,
      "end": 55238,
      "idx": 28
    },
    {
      "start": 55243,
      "end": 55291,
      "idx": 28
    },
    {
      "start": 63744,
      "end": 64109,
      "idx": 55
    },
    {
      "start": 64112,
      "end": 64217,
      "idx": 55
    },
    {
      "start": 64256,
      "end": 64262
    },
    {
      "start": 64275,
      "end": 64279,
      "idx": 5
    },
    {
      "start": 64285,
      "end": 64285,
      "idx": 6
    },
    {
      "start": 64287,
      "end": 64296,
      "idx": 6
    },
    {
      "start": 64298,
      "end": 64310,
      "idx": 6
    },
    {
      "start": 64312,
      "end": 64316,
      "idx": 6
    },
    {
      "start": 64318,
      "end": 64318,
      "idx": 6
    },
    {
      "start": 64320,
      "end": 64321,
      "idx": 6
    },
    {
      "start": 64323,
      "end": 64324,
      "idx": 6
    },
    {
      "start": 64326,
      "end": 64335,
      "idx": 6
    },
    {
      "start": 64336,
      "end": 64433,
      "idx": 7
    },
    {
      "start": 64467,
      "end": 64829,
      "idx": 7
    },
    {
      "start": 64848,
      "end": 64911,
      "idx": 7
    },
    {
      "start": 64914,
      "end": 64967,
      "idx": 7
    },
    {
      "start": 65008,
      "end": 65019,
      "idx": 7
    },
    {
      "start": 65136,
      "end": 65140,
      "idx": 7
    },
    {
      "start": 65142,
      "end": 65276,
      "idx": 7
    },
    {
      "start": 65313,
      "end": 65338
    },
    {
      "start": 65345,
      "end": 65370
    },
    {
      "start": 65382,
      "end": 65391,
      "idx": 53
    },
    {
      "start": 65393,
      "end": 65437,
      "idx": 53
    },
    {
      "start": 65440,
      "end": 65470,
      "idx": 28
    },
    {
      "start": 65474,
      "end": 65479,
      "idx": 28
    },
    {
      "start": 65482,
      "end": 65487,
      "idx": 28
    },
    {
      "start": 65490,
      "end": 65495,
      "idx": 28
    },
    {
      "start": 65498,
      "end": 65500,
      "idx": 28
    }
  ]
}
    ''';
    _unicodeData = UncBlocks.fromJson(res, null);
  }
  return _unicodeData;
}
UncBlocks _unicodeData;
