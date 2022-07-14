namespace PetsOptimizer.Genes;

// sort using http://textmechanic.com/text-tools/basic-text-tools/sort-text-lines/
// ReSharper disable IdentifierTypo
public enum PetGenetics
{
    Fighter = 0,
    Defender = 1,
    Forager = 2,
    Fleeter = 3,
    Breeder = 4,
    Special = 5,
    Mercenary = 6,
    Boomer = 7,
    Sniper = 8,
    Amplifier = 9,
    Tsar = 10,
    Rattler = 11,
    Cursory = 12,
    Fastidious = 13,
    Flashy = 14, // TODO This came from Pet6 When foraging, all pets contribute 1.50x more Foraging Speed if there are no Combat Pets in team
    Opticular = 15,
    Monolithic = 16,
    Alchemic = 17,
    Badumdum = 18, // TODO This came from Pet4 When foraging, the pets in the territory above and below contribute 1.20x Foraging Speed
    Defstone = 19,
    Targeter = 20, //TODO When foraging, this pet contributes 5x Foraging Speed if the pet above this one is also a Targeter
    Looter = 21,
    Refiller = 22,
    Eggshell = 23,
    Lazarus = 24,
    Trasher = 25,
    Miasma = 26, // TODO When foraging, all pets on the team contribute 4.00x more Foraging Speed if there aren't two pets of the same type.
    Converter = 27,
    Heavyweight = 28,
    Fastihoop = 29,
    Ninja = 30,
    Superboomer = 31,
    Peapeapod = 32,
    Borger = 33 // TODO When foraging, this pet contributes 10x more foraging speed if the territory above has at least one Forager type (the Green Leaf type) pet.
}