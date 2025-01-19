import { TestBed } from '@angular/core/testing';

import { EncounterAuthoringService } from './encounter-authoring.service';

describe('EncounterAuthoringService', () => {
  let service: EncounterAuthoringService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EncounterAuthoringService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
