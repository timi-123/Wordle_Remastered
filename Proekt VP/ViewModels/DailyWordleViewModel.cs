using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Proekt_VP.Models;

namespace Proekt_VP.ViewModels
{
    public class UnlimitedWordleViewModel : ViewModelBase
    {
        private static readonly string[] wordList =
        {
             "ABACK", "ABASE", "ABATE", "ABBEY", "ABBOT", "ABHOR", "ABIDE", "ABLED",
    "ABODE", "ABORT", "ABOUT", "ABOVE", "ABUSE", "ABYSS", "ACORN", "ACRID",
    "ACTOR", "ACUTE", "ADAGE", "ADAPT", "ADEPT", "ADMIN", "ADMIT", "ADOBE",
    "ADOPT", "ADORE", "ADORN", "ADULT", "AFFIX", "AFIRE", "AFOOT", "AFOUL",
    "AFTER", "AGAIN", "AGAPE", "AGATE", "AGENT", "AGILE", "AGING", "AGLOW",
    "AGONY", "AGORA", "AGREE", "AHEAD", "AIDER", "AISLE", "ALARM", "ALBUM",
    "ALERT", "ALGAE", "ALIBI", "ALIEN", "ALIGN", "ALIKE", "ALIVE", "ALLAY",
    "ALLEY", "ALLOT", "ALLOW", "ALLOY", "ALOFT", "ALONE", "ALONG", "ALOOF",
    "ALOUD", "ALPHA", "ALTAR", "ALTER", "AMASS", "AMAZE", "AMBER", "AMBLE",
    "AMEND", "AMISS", "AMITY", "AMONG", "AMPLE", "AMPLY", "AMUSE", "ANGEL",
    "ANGER", "ANGLE", "ANGRY", "ANGST", "ANIME", "ANKLE", "ANNEX", "ANNOY",
    "ANNUL", "ANODE", "ANTIC", "ANVIL", "AORTA", "APART", "APHID", "APING",
    "APNEA", "APPLE", "APPLY", "APRON", "APTLY", "ARBOR", "ARDOR", "ARENA",
    "ARGUE", "ARISE", "ARMOR", "AROMA", "AROSE", "ARRAY", "ARROW", "ARSON",
    "ARTSY", "ASCOT", "ASHEN", "ASIDE", "ASKEW", "ASSAY", "ASSET", "ATOLL",
    "ATONE", "ATTIC", "AUDIO", "AUDIT", "AUGUR", "AUNTY", "AVAIL", "AVERT",
    "AVIAN", "AVOID", "AWAIT", "AWAKE", "AWARD", "AWARE", "AWASH", "AWFUL",
    "AWOKE", "AXIAL", "AXIOM", "AXION", "AZURE", "BACON", "BADGE", "BADLY",
    "BAGEL", "BAGGY", "BAKER", "BALER", "BALMY", "BALSA", "BANAL", "BANJO",
    "BARGE", "BARON", "BASAL", "BASIC", "BASIL", "BASIN", "BASIS", "BASTE",
    "BATCH", "BATHE", "BATON", "BATTY", "BAWDY", "BAYOU", "BEACH", "BEADY",
    "BEARD", "BEAST", "BEAUT", "BEECH", "BEEFY", "BEFIT", "BEGAN", "BEGAT",
    "BEGET", "BEGIN", "BEGUN", "BEING", "BELCH", "BELIE", "BELLE", "BELLY",
    "BELOW", "BENCH", "BERET", "BERRY", "BERTH", "BESET", "BETEL", "BEVEL",
    "BEZEL", "BIBLE", "BICEP", "BIDDY", "BIGOT", "BILGE", "BILLY", "BINGE",
    "BINGO", "BIOME", "BIRCH", "BIRTH", "BISON", "BITTY", "BLACK", "BLADE",
    "BLAME", "BLAND", "BLANK", "BLARE", "BLAST", "BLAZE", "BLEAK", "BLEAT",
    "BLEED", "BLEEP", "BLEND", "BLESS", "BLIMP", "BLIND", "BLINK", "BLISS",
    "BLITZ", "BLOAT", "BLOCK", "BLOKE", "BLOND", "BLOOD", "BLOOM", "BLOWN",
    "BLUER", "BLUFF", "BLUNT", "BLURB", "BLURT", "BLUSH", "BOARD", "BOAST",
    "BOBBY", "BONEY", "BONGO", "BONUS", "BOOBY", "BOOST", "BOOTH", "BOOTY",
    "BOOZE", "BOOZY", "BORAX", "BORNE", "BOSOM", "BOSSY", "BOTCH", "BOUGH",
    "BOULE", "BOUND", "BOWEL", "BOXER", "BRACE", "BRAID", "BRAIN", "BRAKE",
    "BRAND", "BRASH", "BRASS", "BRAVE", "BRAVO", "BRAWL", "BRAWN", "BREAD",
    "BREAK", "BREED", "BRIAR", "BRIBE", "BRICK", "BRIDE", "BRIEF", "BRINE",
    "BRING", "BRINK", "BRINY", "BRISK", "BROAD", "BROIL", "BROKE", "BROOD",
    "BROOK", "BROOM", "BROTH", "BROWN", "BRUNT", "BRUSH", "BRUTE", "BUDDY",
    "BUDGE", "BUGGY", "BUGLE", "BUILD", "BUILT", "BULGE", "BULKY", "BULLY",
    "BUNCH", "BUNNY", "BURLY", "BURNT", "BURST", "BUSED", "BUSHY", "BUTCH",
    "BUTTE", "BUXOM", "BUYER", "BYLAW", "CABAL", "CABBY", "CABIN", "CABLE",
    "CACAO", "CACHE", "CACTI", "CADDY", "CADET", "CAGEY", "CAIRN", "CAMEL",
    "CAMEO", "CANAL", "CANDY", "CANNY", "CANOE", "CANON", "CAPER", "CAPUT",
    "CARAT", "CARGO", "CAROL", "CAROM", "CARRY", "CARVE", "CASTE", "CATCH",
    "CATER", "CATTY", "CAULK", "CAUSE", "CAVIL", "CEASE", "CEDAR", "CELLO",
    "CHAFE", "CHAFF", "CHAIN", "CHAIR", "CHALK", "CHAMP", "CHANT", "CHAOS",
    "CHARD", "CHARM", "CHART", "CHASE", "CHASM", "CHEAP", "CHEAT", "CHECK",
    "CHEEK", "CHEER", "CHESS", "CHEST", "CHICK", "CHIDE", "CHIEF", "CHILD",
    "CHILI", "CHILL", "CHIME", "CHINA", "CHIRP", "CHOCK", "CHOIR", "CHOKE",
    "CHORD", "CHORE", "CHOSE", "CHUCK", "CHUMP", "CHUNK", "CHURN", "CHUTE",
    "CIDER", "CIGAR", "CINCH", "CIRCA", "CIVIC", "CIVIL", "CLACK", "CLAIM",
    "CLAMP", "CLANG", "CLANK", "CLASH", "CLASP", "CLASS", "CLEAN", "CLEAR",
    "CLEAT", "CLEFT", "CLERK", "CLICK", "CLIFF", "CLIMB", "CLING", "CLINK",
    "CLOAK", "CLOCK", "CLONE", "CLOSE", "CLOTH", "CLOUD", "CLOUT", "CLOVE",
    "CLOWN", "CLUCK", "CLUED", "CLUMP", "CLUNG", "COACH", "COAST", "COBRA",
    "COCOA", "COLON", "COLOR", "COMET", "COMFY", "COMIC", "COMMA", "CONCH",
    "CONDO", "CONIC", "COPSE", "CORAL", "CORER", "CORNY", "COUCH", "COUGH",
    "COULD", "COUNT", "COUPE", "COURT", "COVEN", "COVER", "COVET", "COVEY",
    "COWER", "COYLY", "CRACK", "CRAFT", "CRAMP", "CRANE", "CRANK", "CRASH",
    "CRASS", "CRATE", "CRAVE", "CRAWL", "CRAZE", "CRAZY", "CREAK", "CREAM",
    "CREDO", "CREED", "CREEK", "CREEP", "CREME", "CREPE", "CREPT", "CRESS",
    "CREST", "CRICK", "CRIED", "CRIER", "CRIME", "CRIMP", "CRISP", "CROAK",
    "CROCK", "CRONE", "CRONY", "CROOK", "CROSS", "CROUP", "CROWD", "CROWN",
    "CRUDE", "CRUEL", "CRUMB", "CRUMP", "CRUSH", "CRUST", "CRYPT", "CUBIC",
    "CUBIT", "CUMIN", "CURIO", "CURLY", "CURRY", "CURSE", "CURVE", "CURVY",
    "CUTIE", "CYBER", "CYCLE", "CYNIC", "DADDY", "DAILY", "DAIRY", "DAISY",
    "DALLY", "DANCE", "DANDY", "DATUM", "DAUNT", "DEALT", "DEATH", "DEBAR",
    "DEBIT", "DEBUG", "DEBUT", "DECAL", "DECAY", "DECOR", "DECOY", "DECRY",
    "DEFER", "DEIGN", "DEITY", "DELAY", "DELTA", "DELVE", "DEMON", "DEMUR",
    "DENIM", "DENSE", "DEPOT", "DEPTH", "DERBY", "DETER", "DETOX", "DEUCE",
    "DEVIL", "DIARY", "DICEY", "DIGIT", "DILLY", "DIMLY", "DINER", "DINGO",
    "DINGY", "DIODE", "DIRGE", "DIRTY", "DISCO", "DITCH", "DITTO", "DITTY",
    "DIVER", "DIZZY", "DODGE", "DODGY", "DOGMA", "DOING", "DOLLY", "DONOR",
    "DONUT", "DOPEY", "DOUBT", "DOUGH", "DOWDY", "DOWEL", "DOWNY", "DOWRY",
    "DOZEN", "DRAFT", "DRAIN", "DRAKE", "DRAMA", "DRANK", "DRAPE", "DRAWL",
    "DRAWN", "DREAD", "DREAM", "DRESS", "DRIED", "DRIER", "DRIFT", "DRILL",
    "DRINK", "DRIVE", "DROIT", "DROLL", "DRONE", "DROOL", "DROOP", "DROSS",
    "DROVE", "DROWN", "DRUID", "DRUNK", "DRYER", "DRYLY", "DUCHY", "DULLY",
    "DUMMY", "DUMPY", "DUNCE", "DUSKY", "DUSTY", "DUTCH", "DUVET", "DWARF",
    "DWELL", "DWELT", "DYING", "EAGER", "EAGLE", "EARLY", "EARTH", "EASEL",
    "EATEN", "EATER", "EBONY", "ECLAT", "EDICT", "EDIFY", "EERIE", "EGRET",
    "EIGHT", "EJECT", "EKING", "ELATE", "ELBOW", "ELDER", "ELECT", "ELEGY",
    "ELFIN", "ELIDE", "ELITE", "ELOPE", "ELUDE", "EMAIL", "EMBED", "EMBER",
    "EMCEE", "EMOJI", "EMPTY", "ENACT", "ENDOW", "ENEMA", "ENEMY", "ENJOY",
    "ENNUI", "ENSUE", "ENTER", "ENTRY", "ENVOY", "EPOCH", "EPOXY", "EQUAL",
    "EQUIP", "ERASE", "ERECT", "ERODE", "ERROR", "ERUPT", "ESSAY", "ESTER",
    "ETHER", "ETHIC", "ETHOS", "ETUDE", "EVADE", "EVENT", "EVERY", "EVICT",
    "EVOKE", "EXACT", "EXALT", "EXCEL", "EXERT", "EXILE", "EXIST", "EXPEL",
    "EXTOL", "EXTRA", "EXULT", "EYING", "FABLE", "FACET", "FAINT", "FAIRY",
    "FAITH", "FALSE", "FANCY", "FANNY", "FARCE", "FATAL", "FATTY", "FAULT",
    "FAUNA", "FAVOR", "FEAST", "FECAL", "FEIGN", "FELLA", "FELON", "FEMME",
    "FEMUR", "FENCE", "FERAL", "FERRY", "FETAL", "FETCH", "FETID", "FETUS",
    "FEVER", "FEWER", "FIBER", "FIBRE", "FICUS", "FIELD", "FIEND", "FIERY",
    "FIFTH", "FIFTY", "FIGHT", "FILER", "FILET", "FILLY", "FILMY", "FILTH",
    "FINAL", "FINCH", "FINER", "FIRST", "FISHY", "FIXER", "FIZZY", "FJORD",
    "FLACK", "FLAIL", "FLAIR", "FLAKE", "FLAKY", "FLAME", "FLANK", "FLARE",
    "FLASH", "FLASK", "FLECK", "FLEET", "FLESH", "FLICK", "FLIER", "FLING",
    "FLINT", "FLIRT", "FLOAT", "FLOCK", "FLOOD", "FLOOR", "FLORA", "FLOSS",
    "FLOUR", "FLOUT", "FLOWN", "FLUFF", "FLUID", "FLUKE", "FLUME", "FLUNG",
    "FLUNK", "FLUSH", "FLUTE", "FLYER", "FOAMY", "FOCAL", "FOCUS", "FOGGY",
    "FOIST", "FOLIO", "FOLLY", "FORAY", "FORCE", "FORGE", "FORGO", "FORTE",
    "FORTH", "FORTY", "FORUM", "FOUND", "FOYER", "FRAIL", "FRAME", "FRANK",
    "FRAUD", "FREAK", "FREED", "FREER", "FRESH", "FRIAR", "FRIED", "FRILL",
    "FRISK", "FRITZ", "FROCK", "FROND", "FRONT", "FROST", "FROTH", "FROWN",
    "FROZE", "FRUIT", "FUDGE", "FUGUE", "FULLY", "FUNGI", "FUNKY", "FUNNY",
    "FUROR", "FURRY", "FUSSY", "FUZZY", "GAFFE", "GAILY", "GAMER", "GAMMA",
    "GAMUT", "GASSY", "GAUDY", "GAUGE", "GAUNT", "GAUZE", "GAVEL", "GAWKY",
    "GAYER", "GAYLY", "GAZER", "GECKO", "GEEKY", "GEESE", "GENIE", "GENRE",
    "GHOST", "GHOUL", "GIANT", "GIDDY", "GIPSY", "GIRLY", "GIRTH", "GIVEN",
    "GIVER", "GLADE", "GLAND", "GLARE", "GLASS", "GLAZE", "GLEAM", "GLEAN",
    "GLIDE", "GLINT", "GLOAT", "GLOBE", "GLOOM", "GLORY", "GLOSS", "GLOVE",
    "GLYPH", "GNASH", "GNOME", "GODLY", "GOING", "GOLEM", "GOLLY", "GONAD",
    "GONER", "GOODY", "GOOEY", "GOOFY", "GOOSE", "GORGE", "GOUGE", "GOURD",
    "GRACE", "GRADE", "GRAFT", "GRAIL", "GRAIN", "GRAND", "GRANT", "GRAPE",
    "GRAPH", "GRASP", "GRASS", "GRATE", "GRAVE", "GRAVY", "GRAZE", "GREAT",
    "GREED", "GREEN", "GREET", "GRIEF", "GRILL", "GRIME", "GRIMY", "GRIND",
    "GRIPE", "GROAN", "GROIN", "GROOM", "GROPE", "GROSS", "GROUP", "GROUT",
    "GROVE", "GROWL", "GROWN", "GRUEL", "GRUFF", "GRUNT", "GUANO", "GUARD",
    "GUAVA", "GUESS", "GUEST", "GUIDE", "GUILD", "GUILE", "GUILT", "GUISE",
    "GULCH", "GULLY", "GUMBO", "GUMMY", "GUPPY", "GUSTO", "GUSTY", "GYPSY",
    "HABIT", "HAIRY", "HALVE", "HANDY", "HAPPY", "HARDY", "HAREM", "HARPY",
    "HARRY", "HARSH", "HASTE", "HASTY", "HATCH", "HATER", "HAUNT", "HAUTE",
    "HAVEN", "HAVOC", "HAZEL", "HEADY", "HEARD", "HEART", "HEATH", "HEAVE",
    "HEAVY", "HEDGE", "HEFTY", "HEIST", "HELIX", "HELLO", "HENCE", "HERON",
    "HILLY", "HINGE", "HIPPO", "HIPPY", "HITCH", "HOARD", "HOBBY", "HOIST",
    "HOLLY", "HOMER", "HONEY", "HONOR", "HORDE", "HORNY", "HORSE", "HOTEL",
    "HOTLY", "HOUND", "HOUSE", "HOVEL", "HOVER", "HOWDY", "HUMAN", "HUMID",
    "HUMOR", "HUMPH", "HUMUS", "HUNCH", "HUNKY", "HURRY", "HUSKY", "HUSSY",
    "HUTCH", "HYDRO", "HYENA", "HYMEN", "HYPER", "ICILY", "ICING", "IDEAL",
    "IDIOM", "IDIOT", "IDLER", "IDYLL", "IGLOO", "ILIAC", "IMAGE", "IMBUE",
    "IMPEL", "IMPLY", "INANE", "INBOX", "INCUR", "INDEX", "INDIE", "INEPT",
    "INERT", "INFER", "INGOT", "INLAY", "INLET", "INNER", "INPUT", "INTER",
    "INTRO", "IONIC", "IRATE", "IRONY", "ISLET", "ISSUE", "ITCHY", "IVORY",
    "JAUNT", "JAZZY", "JELLY", "JERKY", "JETTY", "JEWEL", "JIFFY", "JOINT",
    "JOIST", "JOKER", "JOLLY", "JOUST", "JUDGE", "JUICE", "JUICY", "JUMBO",
    "JUMPY", "JUNTA", "JUNTO", "JUROR", "KAPPA", "KARMA", "KAYAK", "KAZOO",
    "KEBAB", "KHAKI", "KINKY", "KIOSK", "KITTY", "KNACK", "KNAVE", "KNEAD",
    "KNEED", "KNEEL", "KNELT", "KNIFE", "KNOCK", "KNOLL", "KNOWN", "KOALA",
    "KRILL", "LABEL", "LABOR", "LADEN", "LADLE", "LAGER", "LANCE", "LANKY",
    "LAPEL", "LAPSE", "LARGE", "LARVA", "LASER", "LASSO", "LATCH", "LATER",
    "LATHE", "LATTE", "LAUGH", "LAYER", "LEACH", "LEAFY", "LEAKY", "LEANT",
    "LEAPT", "LEARN", "LEASE", "LEASH", "LEAST", "LEAVE", "LEDGE", "LEECH",
    "LEERY", "LEFTY", "LEGAL", "LEGGY", "LEMON", "LEMUR", "LEPER", "LEVEL",
    "LEVER", "LIBEL", "LIEGE", "LIGHT", "LIKEN", "LILAC", "LIMBO", "LIMIT",
    "LINEN", "LINER", "LINGO", "LIPID", "LITHE", "LIVER", "LIVID", "LLAMA",
    "LOAMY", "LOATH", "LOBBY", "LOCAL", "LOCUS", "LODGE", "LOFTY", "LOGIC",
    "LOGIN", "LOOPY", "LOOSE", "LORIS", "LORRY", "LOSER", "LOUSE", "LOUSY",
    "LOVER", "LOWER", "LOWLY", "LOYAL", "LUCID", "LUCKY", "LUMEN", "LUMPY",
    "LUNAR", "LUNCH", "LUNGE", "LUPUS", "LURCH", "LURID", "LUSTY", "LYING",
    "LYMPH", "LYNCH", "LYRIC", "MACAW", "MACHO", "MACRO", "MADAM", "MADLY",
    "MAFIA", "MAGIC", "MAGMA", "MAIZE", "MAJOR", "MAKER", "MAMBO", "MAMMA",
    "MAMMY", "MANGA", "MANGE", "MANGO", "MANGY", "MANIA", "MANIC", "MANLY",
    "MANOR", "MAPLE", "MARCH", "MARRY", "MARSH", "MASON", "MASSE", "MATCH",
    "MATEY", "MAUVE", "MAXIM", "MAYBE", "MAYOR", "MEALY", "MEANT", "MEATY",
    "MECCA", "MEDAL", "MEDIA", "MEDIC", "MELEE", "MELON", "MERCY", "MERGE",
    "MERIT", "MERRY", "METAL", "METER", "METRO", "MICRO", "MIDGE", "MIDST",
    "MIGHT", "MILKY", "MIMIC", "MINCE", "MINER", "MINIM", "MINOR", "MINTY",
    "MINUS", "MIRTH", "MISER", "MISSY", "MOCHA", "MODAL", "MODEL", "MODEM",
    "MOGUL", "MOIST", "MOLAR", "MOLDY", "MOMMY", "MONEY", "MONTH", "MOODY",
    "MOOSE", "MORAL", "MORON", "MORPH", "MOSSY", "MOTEL", "MOTIF", "MOTOR",
    "MOTTO", "MOULT", "MOUND", "MOUNT", "MOURN", "MOUSE", "MOUTH", "MOVER",
    "MOVIE", "MOWER", "MUCKY", "MUCUS", "MUDDY", "MULCH", "MUMMY", "MUNCH",
    "MURAL", "MURKY", "MUSHY", "MUSIC", "MUSKY", "MUSTY", "MYRRH", "NADIR",
    "NAIVE", "NANNY", "NASAL", "NASTY", "NATAL", "NAVAL", "NAVEL", "NEEDY",
    "NEIGH", "NERDY", "NERVE", "NERVY", "NEVER", "NEWER", "NEWLY", "NICER",
    "NICHE", "NIECE", "NIGHT", "NINJA", "NINNY", "NINTH", "NOBLE", "NOBLY",
    "NOISE", "NOISY", "NOMAD", "NOOSE", "NORTH", "NOSEY", "NOTCH", "NOVEL",
    "NUDGE", "NURSE", "NUTTY", "NYLON", "NYMPH", "OAKEN", "OASIS", "OBESE",
    "OCCUR", "OCEAN", "OCTAL", "OCTET", "ODDER", "ODDLY", "OFFAL", "OFFER",
    "OFTEN", "OLDEN", "OLDER", "OLIVE", "OMBRE", "OMEGA", "ONION", "ONSET",
    "OPERA", "OPINE", "OPIUM", "OPTIC", "ORBIT", "ORDER", "ORGAN", "OTHER",
    "OTTER", "OUGHT", "OUNCE", "OUTDO", "OUTER", "OUTGO", "OVARY", "OVATE",
    "OVERT", "OVINE", "OVOID", "OWING", "OWNER", "OXIDE", "OZONE", "PADDY",
    "PAGAN", "PAINT", "PALER", "PALSY", "PANEL", "PANIC", "PANSY", "PAPAL",
    "PAPER", "PARER", "PARKA", "PARRY", "PARSE", "PARTY", "PASTA", "PASTE",
    "PASTY", "PATCH", "PATIO", "PATSY", "PATTY", "PAUSE", "PAYEE", "PAYER",
    "PEACE", "PEACH", "PEARL", "PECAN", "PEDAL", "PENAL", "PENCE", "PENNE",
    "PENNY", "PERCH", "PERIL", "PERKY", "PESKY", "PESTO", "PETAL", "PETTY",
    "PHASE", "PHONE", "PHONY", "PHOTO", "PIANO", "PICKY", "PIECE", "PIETY",
    "PIGGY", "PILOT", "PINCH", "PINEY", "PINKY", "PINTO", "PIOUS", "PIPER",
    "PIQUE", "PITCH", "PITHY", "PIVOT", "PIXEL", "PIXIE", "PIZZA", "PLACE",
    "PLAID", "PLAIN", "PLAIT", "PLANE", "PLANK", "PLANT", "PLATE", "PLAZA",
    "PLEAD", "PLEAT", "PLIED", "PLIER", "PLUCK", "PLUMB", "PLUME", "PLUMP",
    "PLUNK", "PLUSH", "POESY", "POINT", "POISE", "POKER", "POLAR", "POLKA",
    "POLYP", "POOCH", "POPPY", "PORCH", "POSER", "POSIT", "POSSE", "POUCH",
    "POUND", "POUTY", "POWER", "PRANK", "PRAWN", "PREEN", "PRESS", "PRICE",
    "PRICK", "PRIDE", "PRIED", "PRIME", "PRIMO", "PRINT", "PRIOR", "PRISM",
    "PRIVY", "PRIZE", "PROBE", "PRONE", "PRONG", "PROOF", "PROSE", "PROUD",
    "PROVE", "PROWL", "PROXY", "PRUDE", "SWILL", "SWINE", "SWING", "SWIRL",
    "SWISH", "SWOON", "SWOOP", "SWORD", "SWORE", "SWORN", "SWUNG", "SYNOD",
    "SYRUP", "TABBY", "TABLE", "TABOO", "TACIT", "TACKY", "TAFFY", "TAINT",
    "TAKEN", "TAKER", "TALLY", "TALON", "TAMER", "TANGO", "TANGY", "TAPER",
    "TAPIR", "TARDY", "TAROT", "TASTE", "TASTY", "TATTY", "TAUNT", "TAWNY",
    "TEACH", "TEARY", "TEASE", "TEDDY", "TEETH", "TEMPO", "TENET", "TENOR",
    "TENSE", "TENTH", "TEPEE", "TEPID", "TERRA", "TERSE", "TESTY", "THANK",
    "THEFT", "THEIR", "THEME", "THERE", "THESE", "THETA", "THICK", "THIEF",
    "THIGH", "THING", "THINK", "THIRD", "THONG", "THORN", "THOSE", "THREE",
    "THREW", "THROB", "THROW", "THRUM", "THUMB", "THUMP", "THYME", "TIARA",
    "TIBIA", "TIDAL", "TIGER", "TIGHT", "TILDE", "TIMER", "TIMID", "TIPSY",
    "TITAN", "TITHE", "TITLE", "TOADY", "TOAST", "TODAY", "TODDY", "TOKEN",
    "TONAL", "TONGA", "TONIC", "TOOTH", "TOPAZ", "TOPIC", "TORCH", "TORSO",
    "TORUS", "TOTAL", "TOTEM", "TOUCH", "TOUGH", "TOWEL", "TOWER", "TOXIC",
    "TOXIN", "TRACE", "TRACK", "TRACT", "TRADE", "TRAIL", "TRAIN", "TRAIT",
    "TRAMP", "TRASH", "TRAWL", "TREAD", "TREAT", "TREND", "TRIAD", "TRIAL",
    "TRIBE", "TRICE", "TRICK", "TRIED", "TRIPE", "TRITE", "TROLL", "TROOP",
    "TROPE", "TROUT", "TROVE", "TRUCE", "TRUCK", "TRUER", "TRULY", "TRUMP",
    "TRUNK", "TRUSS", "TRUST", "TRUTH", "TRYST", "TUBAL", "TUBER", "TULIP",
    "TULLE", "TUMOR", "TUNIC", "TURBO", "TUTOR", "TWANG", "TWEAK", "TWEED",
    "TWEET", "TWICE", "TWINE", "TWIRL", "TWIST", "TWIXT", "TYING", "UDDER",
    "ULCER", "ULTRA", "UMBRA", "UNCLE", "UNCUT", "UNDER", "UNDID", "UNDUE",
    "UNFED", "UNFIT", "UNIFY", "UNION", "UNITE", "UNITY", "UNLIT", "UNMET",
    "UNSET", "UNTIE", "UNTIL", "UNWED", "UNZIP", "UPPER", "UPSET", "URBAN",
    "URINE", "USAGE", "USHER", "USING", "USUAL", "USURP", "UTILE", "UTTER",
    "UVULA", "VAGUE", "VALET", "VALID", "VALOR", "VALUE", "VALVE", "VAPID",
    "VAPOR", "VAULT", "VAUNT", "VEGAN", "VENOM", "VENUE", "VERGE", "VERSE",
    "VERSO", "VERVE", "VICAR", "VIDEO", "VIGIL", "VIGOR", "VILLA", "VINYL",
    "VIOLA", "VIPER", "VIRAL", "VIRUS", "VISIT", "VISOR", "VISTA", "VITAL",
    "VIVID", "VIXEN", "VOCAL", "VODKA", "VOGUE", "VOICE", "VOILA", "VOMIT",
    "VOTER", "VOUCH", "VOWEL", "VYING", "WACKY", "WAFER", "WAGER", "WAGON",
    "WAIST", "WAIVE", "WALTZ", "WARTY", "WASTE", "WATCH", "WATER", "WAVER",
    "WAXEN", "WEARY", "WEAVE", "WEDGE", "WEEDY", "WEIGH", "WEIRD", "WELCH",
    "WELSH", "WENCH", "WHACK", "WHALE", "WHARF", "WHEAT", "WHEEL", "WHELP",
    "WHERE", "WHICH", "WHIFF", "WHILE", "WHINE", "WHINY", "WHIRL", "WHISK",
    "WHITE", "WHOLE", "WHOOP", "WHOSE", "WIDEN", "WIDER", "WIDOW", "WIDTH",
    "WIELD", "WIGHT", "WILLY", "WIMPY", "WINCE", "WINCH", "WINDY", "WISER",
    "WISPY", "WITCH", "WITTY", "WOKEN", "WOMAN", "WOMEN", "WOODY", "WOOER",
    "WOOLY", "WOOZY", "WORDY", "WORLD", "WORRY", "WORSE", "WORST", "WORTH",
    "WOULD", "WOUND", "WOVEN", "WRACK", "WRATH", "WREAK", "WRECK", "WREST",
    "WRING", "WRIST", "WRITE", "WRONG", "WROTE", "WRUNG", "WRYLY", "YACHT",
    "YEARN", "YEAST", "YIELD", "YOUNG", "YOUTH", "ZEBRA", "ZESTY", "ZONAL",

        };

        private static readonly Random random = new Random();

        private string targetWord = "";
        private int currentRow;
        private int currentColumn;
        private bool isSubmitting;

        public ObservableCollection<ObservableCollection<LetterCell>> Guesses { get; }
        public ObservableCollection<KeyModel> KeyboardKeys { get; }
        public ObservableCollection<ObservableCollection<KeyModel>> KeyboardRows { get; }

        private string errorMessage = "";

        public string ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }

        private bool isGameOver;

        public bool IsGameOver
        {
            get => isGameOver;
            set
            {
                SetProperty(ref isGameOver, value);
                OnPropertyChanged(nameof(IsGameActive));
            }
        }

        public bool IsGameActive => !IsGameOver;

        private int gamesPlayed;

        public int GamesPlayed
        {
            get => gamesPlayed;
            set
            {
                SetProperty(ref gamesPlayed, value);
                OnPropertyChanged(nameof(WinPercentage));
            }
        }

        private int gamesWon;

        public int GamesWon
        {
            get => gamesWon;
            set
            {
                SetProperty(ref gamesWon, value);
                OnPropertyChanged(nameof(WinPercentage));
            }
        }

        private int gamesLost;

        public int GamesLost
        {
            get => gamesLost;
            set => SetProperty(ref gamesLost, value);
        }

        private int wonIn1;
        private int wonIn2;
        private int wonIn3;
        private int wonIn4;
        private int wonIn5;
        private int wonIn6;

        public int WonIn1
        {
            get => wonIn1;
            set
            {
                SetProperty(ref wonIn1, value);
                UpdateDistribution();
            }
        }

        public int WonIn2
        {
            get => wonIn2;
            set
            {
                SetProperty(ref wonIn2, value);
                UpdateDistribution();
            }
        }

        public int WonIn3
        {
            get => wonIn3;
            set
            {
                SetProperty(ref wonIn3, value);
                UpdateDistribution();
            }
        }

        public int WonIn4
        {
            get => wonIn4;
            set
            {
                SetProperty(ref wonIn4, value);
                UpdateDistribution();
            }
        }

        public int WonIn5
        {
            get => wonIn5;
            set
            {
                SetProperty(ref wonIn5, value);
                UpdateDistribution();
            }
        }

        public int WonIn6
        {
            get => wonIn6;
            set
            {
                SetProperty(ref wonIn6, value);
                UpdateDistribution();
            }
        }

        public double WinPercentage
        {
            get
            {
                if (GamesPlayed == 0)
                    return 0;

                return (double)GamesWon / GamesPlayed * 100;
            }
        }

        public double Distribution1Width => CalculateBarWidth(WonIn1);
        public double Distribution2Width => CalculateBarWidth(WonIn2);
        public double Distribution3Width => CalculateBarWidth(WonIn3);
        public double Distribution4Width => CalculateBarWidth(WonIn4);
        public double Distribution5Width => CalculateBarWidth(WonIn5);
        public double Distribution6Width => CalculateBarWidth(WonIn6);

        private int MaxDistributionValue
        {
            get
            {
                return new[]
                {
                    WonIn1,
                    WonIn2,
                    WonIn3,
                    WonIn4,
                    WonIn5,
                    WonIn6
                }.Max();
            }
        }

        public ICommand EnterLetterCommand { get; }
        public ICommand SubmitGuessCommand { get; }
        public ICommand BackspaceCommand { get; }
        public ICommand NewGameCommand { get; }

        public UnlimitedWordleViewModel()
        {
            Guesses =
                new ObservableCollection<ObservableCollection<LetterCell>>();

            for (int i = 0; i < 6; i++)
            {
                var row = new ObservableCollection<LetterCell>();

                for (int j = 0; j < 5; j++)
                {
                    row.Add(new LetterCell());
                }

                Guesses.Add(row);
            }

            KeyboardKeys = new ObservableCollection<KeyModel>();

            KeyboardRows =
                new ObservableCollection<ObservableCollection<KeyModel>>();

            CreateKeyboard();

            EnterLetterCommand =
                new RelayCommand(OnEnterLetter);

            SubmitGuessCommand =
                new RelayCommand(_ => SubmitGuess());

            BackspaceCommand =
                new RelayCommand(_ => Backspace());

            NewGameCommand =
                new RelayCommand(_ => StartNewGame());

            StartNewGame();
        }

        private void CreateKeyboard()
        {
            AddKeyboardRow("QWERTYUIOP");
            AddKeyboardRow("ASDFGHJKL");

            var lastRow = new ObservableCollection<KeyModel>();

            var enterKey = new KeyModel
            {
                Key = "ENTER"
            };

            KeyboardKeys.Add(enterKey);
            lastRow.Add(enterKey);

            foreach (char letter in "ZXCVBNM")
            {
                var key = new KeyModel
                {
                    Key = letter.ToString()
                };

                KeyboardKeys.Add(key);
                lastRow.Add(key);
            }

            var backspaceKey = new KeyModel
            {
                Key = "⌫"
            };

            KeyboardKeys.Add(backspaceKey);
            lastRow.Add(backspaceKey);

            KeyboardRows.Add(lastRow);
        }

        private void AddKeyboardRow(string letters)
        {
            var row = new ObservableCollection<KeyModel>();

            foreach (char letter in letters)
            {
                var key = new KeyModel
                {
                    Key = letter.ToString()
                };

                KeyboardKeys.Add(key);
                row.Add(key);
            }

            KeyboardRows.Add(row);
        }

        public void StartNewGame()
        {
            string oldWord = targetWord;

            do
            {
                targetWord =
                    wordList[random.Next(wordList.Length)];
            }
            while (wordList.Length > 1 &&
                   targetWord == oldWord);

            foreach (var row in Guesses)
            {
                foreach (var cell in row)
                {
                    cell.Letter = "";
                    cell.BackgroundColor =
                        Brushes.Transparent;
                }
            }

            foreach (var key in KeyboardKeys)
            {
                key.BackgroundColor =
                    Brushes.LightGray;
            }

            currentRow = 0;
            currentColumn = 0;
            isSubmitting = false;

            ErrorMessage = "";
            IsGameOver = false;
        }

        private void OnEnterLetter(object? parameter)
        {
            if (IsGameOver ||
                isSubmitting ||
                parameter is not string key)
            {
                return;
            }

            ErrorMessage = "";

            if (key == "ENTER")
            {
                SubmitGuess();
                return;
            }

            if (key == "⌫" || key == "BACK")
            {
                Backspace();
                return;
            }

            if (key.Length != 1)
            {
                return;
            }

            if (currentRow < 6 &&
                currentColumn < 5)
            {
                Guesses[currentRow][currentColumn].Letter =
                    key.ToUpper();

                currentColumn++;
            }
        }

        private void Backspace()
        {
            if (IsGameOver || isSubmitting)
            {
                return;
            }

            ErrorMessage = "";

            if (currentColumn > 0)
            {
                currentColumn--;

                Guesses[currentRow][currentColumn].Letter = "";
            }
        }

        private async void SubmitGuess()
        {
            if (IsGameOver || isSubmitting)
            {
                return;
            }

            if (currentColumn != 5)
            {
                ErrorMessage =
                    "Enter a five-letter word.";

                return;
            }

            isSubmitting = true;

            try
            {
                string guess = string.Join(
                    "",
                    Guesses[currentRow]
                        .Select(cell => cell.Letter)
                );

                bool isValid =
                    await Services.WordValidator
                        .IsValidWordAsync(guess);

                if (!isValid)
                {
                    ErrorMessage = "Not in word list";
                    return;
                }

                EvaluateGuess(guess);

                if (guess == targetWord)
                {
                    int attempt = currentRow + 1;

                    GamesPlayed++;
                    GamesWon++;

                    AddWinToStatistics(attempt);

                    ErrorMessage =
                        $"Correct! You guessed the word in " +
                        $"{attempt} attempt(s).";

                    IsGameOver = true;
                    return;
                }

                currentRow++;
                currentColumn = 0;

                if (currentRow == 6)
                {
                    GamesPlayed++;
                    GamesLost++;

                    ErrorMessage =
                        $"The word was {targetWord}";

                    IsGameOver = true;
                }
            }
            catch
            {
                ErrorMessage =
                    "An error occurred while checking the word.";
            }
            finally
            {
                isSubmitting = false;
            }
        }

        private void AddWinToStatistics(int attempt)
        {
            switch (attempt)
            {
                case 1:
                    WonIn1++;
                    break;

                case 2:
                    WonIn2++;
                    break;

                case 3:
                    WonIn3++;
                    break;

                case 4:
                    WonIn4++;
                    break;

                case 5:
                    WonIn5++;
                    break;

                case 6:
                    WonIn6++;
                    break;
            }
        }

        private double CalculateBarWidth(int value)
        {
            int maximumValue = MaxDistributionValue;

            if (maximumValue == 0)
            {
                return 30;
            }

            const double minimumWidth = 30;
            const double additionalWidth = 170;

            return minimumWidth +
                   (double)value / maximumValue *
                   additionalWidth;
        }

        private void UpdateDistribution()
        {
            OnPropertyChanged(nameof(Distribution1Width));
            OnPropertyChanged(nameof(Distribution2Width));
            OnPropertyChanged(nameof(Distribution3Width));
            OnPropertyChanged(nameof(Distribution4Width));
            OnPropertyChanged(nameof(Distribution5Width));
            OnPropertyChanged(nameof(Distribution6Width));
        }

        private void EvaluateGuess(string guess)
        {
            List<char> remainingLetters =
                targetWord.ToList();

            for (int i = 0; i < 5; i++)
            {
                if (guess[i] == targetWord[i])
                {
                    remainingLetters.Remove(guess[i]);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                char guessedLetter = guess[i];

                LetterCell cell =
                    Guesses[currentRow][i];

                Brush color;

                if (guessedLetter == targetWord[i])
                {
                    color = Brushes.Green;
                }
                else if (remainingLetters.Contains(guessedLetter))
                {
                    color = Brushes.Goldenrod;
                    remainingLetters.Remove(guessedLetter);
                }
                else
                {
                    color = Brushes.DarkGray;
                }

                cell.BackgroundColor = color;

                UpdateKeyboardColor(
                    guessedLetter.ToString(),
                    color);
            }
        }

        private void UpdateKeyboardColor(
            string letter,
            Brush color)
        {
            KeyModel? key =
                KeyboardKeys.FirstOrDefault(
                    keyboardKey =>
                        keyboardKey.Key == letter);

            if (key == null)
            {
                return;
            }

            if (color == Brushes.Green)
            {
                key.BackgroundColor =
                    Brushes.Green;
            }
            else if (color == Brushes.Goldenrod &&
                     key.BackgroundColor != Brushes.Green)
            {
                key.BackgroundColor =
                    Brushes.Goldenrod;
            }
            else if (color == Brushes.DarkGray &&
                     key.BackgroundColor != Brushes.Green &&
                     key.BackgroundColor != Brushes.Goldenrod)
            {
                key.BackgroundColor =
                    Brushes.DarkGray;
            }
        }
    }
}