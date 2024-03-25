using System;
using Confab.Modules.Attendances.Domain.Entities;
using Confab.Modules.Attendances.Domain.Exceptions;
using Confab.Modules.Attendances.Domain.Types;
using System.Linq;
using Shouldly;
using Xunit;

namespace Confab.Modules.Attendances.Tests.Unit.Entities
{
	public class My_Participant_Attend_Tests
    {
        private void Act(Attendance attendance) => _participant.Attend(attendance);

        [Fact]
		public void given_already_participated_attendable_event_id_attend_should_fail()
		{
			var attendableEventId = Guid.NewGuid();
			var from = new DateTime(2024, 03, 17, 9, 0, 0);
            var to = new DateTime(2024, 03, 17, 10, 0, 0);
            var attendance1 = new Attendance(Guid.NewGuid(), attendableEventId, Guid.NewGuid(), _participant.Id, from, to);
            var attendance2= new Attendance(Guid.NewGuid(), attendableEventId, Guid.NewGuid(), _participant.Id, from, to);

            _participant.Attend(attendance1);
            var exception = Record.Exception(() => Act(attendance2));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<AlreadyParticipatingInEventException>();
        }

        [Fact]
        public void given_colliding_attendable_events_attend_should_fail()
        {
            var from1 = new DateTime(2024, 03, 17, 9, 0, 0);
            var to1 = new DateTime(2024, 03, 17, 10, 0, 0);
            var from2 = new DateTime(2024, 03, 17, 9, 30, 0);
            var to2 = new DateTime(2024, 03, 17, 11, 0, 0);
            var attendance1 = new Attendance(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), _participant.Id, from1, to1);
            var attendance2 = new Attendance(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), _participant.Id, from2, to2);

            _participant.Attend(attendance1);
            var exception = Record.Exception(() => Act(attendance2));

            exception.ShouldNotBeNull();
            exception.ShouldBeOfType<AlreadyParticipatingSameTimeException>();
        }

        [Fact]
        public void given_non_colliding_attendable_events_attend_should_succeed()
        {
            var from1 = new DateTime(2024, 03, 17, 9, 0, 0);
            var to1 = new DateTime(2024, 03, 17, 10, 0, 0);
            var from2 = new DateTime(2024, 03, 17, 10, 00, 0);
            var to2 = new DateTime(2024, 03, 17, 11,30, 0);
            var attendance1 = new Attendance(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), _participant.Id, from1, to1);
            var attendance2 = new Attendance(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), _participant.Id, from2, to2);

            _participant.Attend(attendance1);
            Act(attendance2);

            _participant.Attendances.Last().ShouldBe(attendance2);
        }

        private readonly Participant _participant;
        private readonly Guid _conferenceId = Guid.NewGuid();
        private readonly Guid _userId = Guid.NewGuid();

        public My_Participant_Attend_Tests()
		{
			_participant = new Participant(Guid.NewGuid(), _conferenceId, _userId);
		}

	}
}

