import { Selector } from 'testcafe';

fixture `Calculator E2E Test`
    .page `http://129.151.223.141/index.html`;

test('Beregning gemmes korrekt', async t => {
    await t
        .click(Selector('button').withText('5'))
        .click(Selector('button').withText('+'))
        .click(Selector('button').withText('3'))
        .click(Selector('button').withText('='))
        .expect(Selector('#display').value).eql('8');

    const historyRow = Selector('#history tr').nth(0);
    await t.expect(historyRow.innerText).contains('5+3');
});
