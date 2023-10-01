import { MouseEventHandler } from "react";
import styled, { css } from "styled-components";

export const StyledCard = styled.div`
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  width: 150px;
  border: 2px solid;
  border-color: var(--primary-color);
  padding: 0.5rem 0.3rem;
  margin: 13px 10px;
  position: relative;
  border-radius: 5px;
`;

export const StyledCardInfoWrapper = styled.h4`
  font-weight: 400;
  text-align: center;
`;

export const StyledImageContainer = styled.div`
  width: 100px;
  height: 100px;
  margin: 0 auto;

  img {
    width: 100%;
    object-fit: cover;
  }
`;

export const StyledButtonContainer = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: center;

  button {
    margin: 0 auto;
  }
`;

export const StyledCardPrice = styled.span`
  text-align: center;
  display: inline-block;
  font-weight: bold;
`;

interface StyledCardBadgeProps {
  $isVisible: boolean;
}

export const StyledCardBadge = styled.div<StyledCardBadgeProps>`
  position: relative;
  top: 3px;
  width: 90px;
  height: 30px;
  background-color: rgb(60, 128, 255);
  border-radius: 50%;
  color: #fff;
  font-weight: bold;
  text-align: center;
  border: 2px solid rgb(21, 0, 138);
  font-size: 20px;
  display: ${(props) => (props.$isVisible ? "block" : "none")};
`;