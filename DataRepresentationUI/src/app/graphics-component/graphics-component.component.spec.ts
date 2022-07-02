import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GraphicsComponentComponent } from './graphics-component.component';

describe('GraphicsComponentComponent', () => {
  let component: GraphicsComponentComponent;
  let fixture: ComponentFixture<GraphicsComponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GraphicsComponentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GraphicsComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
