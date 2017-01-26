import { AngulardotnetcorePage } from './app.po';

describe('angulardotnetcore App', function() {
  let page: AngulardotnetcorePage;

  beforeEach(() => {
    page = new AngulardotnetcorePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
