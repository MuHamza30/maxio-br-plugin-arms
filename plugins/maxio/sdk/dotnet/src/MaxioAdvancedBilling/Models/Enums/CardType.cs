using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// The type of card used.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<CardType>))]
public sealed record CardType : OpenStringEnum<CardType>
{
    private CardType(string value) : base(value)
    {
    }

    public static readonly CardType Visa = new("visa");

    public static readonly CardType Master = new("master");

    public static readonly CardType Elo = new("elo");

    public static readonly CardType Cabal = new("cabal");

    public static readonly CardType Alelo = new("alelo");

    public static readonly CardType Discover = new("discover");

    public static readonly CardType AmericanExpress = new("american_express");

    public static readonly CardType Naranja = new("naranja");

    public static readonly CardType DinersClub = new("diners_club");

    public static readonly CardType Jcb = new("jcb");

    public static readonly CardType Dankort = new("dankort");

    public static readonly CardType Maestro = new("maestro");

    public static readonly CardType MaestroNoLuhn = new("maestro_no_luhn");

    public static readonly CardType Forbrugsforeningen = new("forbrugsforeningen");

    public static readonly CardType Sodexo = new("sodexo");

    public static readonly CardType Alia = new("alia");

    public static readonly CardType Vr = new("vr");

    public static readonly CardType Unionpay = new("unionpay");

    public static readonly CardType Carnet = new("carnet");

    public static readonly CardType CartesBancaires = new("cartes_bancaires");

    public static readonly CardType Olimpica = new("olimpica");

    public static readonly CardType Creditel = new("creditel");

    public static readonly CardType Confiable = new("confiable");

    public static readonly CardType Synchrony = new("synchrony");

    public static readonly CardType Routex = new("routex");

    public static readonly CardType Mada = new("mada");

    public static readonly CardType BpPlus = new("bp_plus");

    public static readonly CardType Passcard = new("passcard");

    public static readonly CardType Edenred = new("edenred");

    public static readonly CardType Anda = new("anda");

    public static readonly CardType TarjetaD = new("tarjeta-d");

    public static readonly CardType Hipercard = new("hipercard");

    public static readonly CardType Bogus = new("bogus");

    public static readonly CardType Switch = new("switch");

    public static readonly CardType Solo = new("solo");

    public static readonly CardType Laser = new("laser");

    public TResult Match<TResult>(Func<TResult> onVisa,
        Func<TResult> onMaster,
        Func<TResult> onElo,
        Func<TResult> onCabal,
        Func<TResult> onAlelo,
        Func<TResult> onDiscover,
        Func<TResult> onAmericanExpress,
        Func<TResult> onNaranja,
        Func<TResult> onDinersClub,
        Func<TResult> onJcb,
        Func<TResult> onDankort,
        Func<TResult> onMaestro,
        Func<TResult> onMaestroNoLuhn,
        Func<TResult> onForbrugsforeningen,
        Func<TResult> onSodexo,
        Func<TResult> onAlia,
        Func<TResult> onVr,
        Func<TResult> onUnionpay,
        Func<TResult> onCarnet,
        Func<TResult> onCartesBancaires,
        Func<TResult> onOlimpica,
        Func<TResult> onCreditel,
        Func<TResult> onConfiable,
        Func<TResult> onSynchrony,
        Func<TResult> onRoutex,
        Func<TResult> onMada,
        Func<TResult> onBpPlus,
        Func<TResult> onPasscard,
        Func<TResult> onEdenred,
        Func<TResult> onAnda,
        Func<TResult> onTarjetaD,
        Func<TResult> onHipercard,
        Func<TResult> onBogus,
        Func<TResult> onSwitch,
        Func<TResult> onSolo,
        Func<TResult> onLaser,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Visa => onVisa(),
            _ when this == Master => onMaster(),
            _ when this == Elo => onElo(),
            _ when this == Cabal => onCabal(),
            _ when this == Alelo => onAlelo(),
            _ when this == Discover => onDiscover(),
            _ when this == AmericanExpress => onAmericanExpress(),
            _ when this == Naranja => onNaranja(),
            _ when this == DinersClub => onDinersClub(),
            _ when this == Jcb => onJcb(),
            _ when this == Dankort => onDankort(),
            _ when this == Maestro => onMaestro(),
            _ when this == MaestroNoLuhn => onMaestroNoLuhn(),
            _ when this == Forbrugsforeningen => onForbrugsforeningen(),
            _ when this == Sodexo => onSodexo(),
            _ when this == Alia => onAlia(),
            _ when this == Vr => onVr(),
            _ when this == Unionpay => onUnionpay(),
            _ when this == Carnet => onCarnet(),
            _ when this == CartesBancaires => onCartesBancaires(),
            _ when this == Olimpica => onOlimpica(),
            _ when this == Creditel => onCreditel(),
            _ when this == Confiable => onConfiable(),
            _ when this == Synchrony => onSynchrony(),
            _ when this == Routex => onRoutex(),
            _ when this == Mada => onMada(),
            _ when this == BpPlus => onBpPlus(),
            _ when this == Passcard => onPasscard(),
            _ when this == Edenred => onEdenred(),
            _ when this == Anda => onAnda(),
            _ when this == TarjetaD => onTarjetaD(),
            _ when this == Hipercard => onHipercard(),
            _ when this == Bogus => onBogus(),
            _ when this == Switch => onSwitch(),
            _ when this == Solo => onSolo(),
            _ when this == Laser => onLaser(),
            _ => otherwise(Value)
        };

    public void Match(Action onVisa,
        Action onMaster,
        Action onElo,
        Action onCabal,
        Action onAlelo,
        Action onDiscover,
        Action onAmericanExpress,
        Action onNaranja,
        Action onDinersClub,
        Action onJcb,
        Action onDankort,
        Action onMaestro,
        Action onMaestroNoLuhn,
        Action onForbrugsforeningen,
        Action onSodexo,
        Action onAlia,
        Action onVr,
        Action onUnionpay,
        Action onCarnet,
        Action onCartesBancaires,
        Action onOlimpica,
        Action onCreditel,
        Action onConfiable,
        Action onSynchrony,
        Action onRoutex,
        Action onMada,
        Action onBpPlus,
        Action onPasscard,
        Action onEdenred,
        Action onAnda,
        Action onTarjetaD,
        Action onHipercard,
        Action onBogus,
        Action onSwitch,
        Action onSolo,
        Action onLaser,
        Action<string> otherwise)
    {
        if (this == Visa) onVisa();
        else if (this == Master) onMaster();
        else if (this == Elo) onElo();
        else if (this == Cabal) onCabal();
        else if (this == Alelo) onAlelo();
        else if (this == Discover) onDiscover();
        else if (this == AmericanExpress) onAmericanExpress();
        else if (this == Naranja) onNaranja();
        else if (this == DinersClub) onDinersClub();
        else if (this == Jcb) onJcb();
        else if (this == Dankort) onDankort();
        else if (this == Maestro) onMaestro();
        else if (this == MaestroNoLuhn) onMaestroNoLuhn();
        else if (this == Forbrugsforeningen) onForbrugsforeningen();
        else if (this == Sodexo) onSodexo();
        else if (this == Alia) onAlia();
        else if (this == Vr) onVr();
        else if (this == Unionpay) onUnionpay();
        else if (this == Carnet) onCarnet();
        else if (this == CartesBancaires) onCartesBancaires();
        else if (this == Olimpica) onOlimpica();
        else if (this == Creditel) onCreditel();
        else if (this == Confiable) onConfiable();
        else if (this == Synchrony) onSynchrony();
        else if (this == Routex) onRoutex();
        else if (this == Mada) onMada();
        else if (this == BpPlus) onBpPlus();
        else if (this == Passcard) onPasscard();
        else if (this == Edenred) onEdenred();
        else if (this == Anda) onAnda();
        else if (this == TarjetaD) onTarjetaD();
        else if (this == Hipercard) onHipercard();
        else if (this == Bogus) onBogus();
        else if (this == Switch) onSwitch();
        else if (this == Solo) onSolo();
        else if (this == Laser) onLaser();
        else otherwise(Value);
    }
}
