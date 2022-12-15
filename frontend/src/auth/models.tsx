/**
 * Response to valid login request.
 */
interface LogInResponse {
  userId: string;
  userName: string;
  accessToken: string;
};

//
export type {
    LogInResponse
}